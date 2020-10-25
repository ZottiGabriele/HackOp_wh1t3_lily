import sys
import os
import json

def main(ch_number: str):
    vfs_path = "C:\\Users\\Zotti\\Documents\\0_UNITY\\HackOp_wh1t3_lily\\Assets\\Resources\\VFS"
    start_path = vfs_path + "\\challenge_" + ch_number + "\\"
    json_path = vfs_path + "\\..\\vfs_ch_" + ch_number + ".json" 
    d = {'childs':path_to_dict("\\".join(start_path.split("\\")[:-1]), start_path, vfs_path)}
    out_file = open(json_path, "w")
    out = json.dumps(d, indent=2)
    out_file.write(out)
    out_file.close()
    print("\n\nOutput json written at " + json_path + "\n\n")
    
def path_to_dict(start_path, path, vfs_path):
    file_name = os.path.basename(path)

    d = {'hidden':False}
    d['readable'] = True
    d['name'] = file_name
    d['full_path'] = path[len(start_path):len(path)-len(os.path.basename(path))].replace("\\","/") + d['name']
    d['flags'] = "-rw-rw-r--"
    d['user'] = "user"
    d['group'] = "group"

    #fix root folder
    if(file_name == ""):
        d['name'] = "/"
        d['full_path'] = "/"
    
    if os.path.isdir(path):
        d['type'] = "directory"
        d['flags'] = "dr-xr-xr-x"
        d['childs'] = []
        for x in os.listdir(path):
            #skip .meta files
            if(not x.__contains__(".meta")):
                d['childs'].append(path_to_dict(start_path, os.path.join(path,x), vfs_path))
    else:
        d['type'] = "file"
        f = open(path, "r")
        d['content'] = f.read()
        f.close()

    parse_custom_tags(d)

    return d

def parse_custom_tags(d):
    d['hidden'] = d['name'].__contains__("_h_")
    d['name'] = d['name'].replace("_h_", ".")
    d['full_path'] = d["full_path"].replace("_h_", ".")

    exe = d['name'].__contains__("_x_")
    if (exe):
        d['name'] = d['name'].replace("_x_", "")
        d['full_path'] = d["full_path"].replace("_x_", "")
        d['flags'] = d['flags'][0:3] + "x" + d['flags'][4:6] + "x" + d['flags'][7:9] + "x"

    root = d['name'].__contains__("_r_")
    if(root):
        d['name'] = d['name'].replace("_r_", "")
        d['full_path'] = d["full_path"].replace("_r_", "")
        d['user'] = "root"
        d['group'] = "root"
        if(d['type'] == "directory"):
            d['flags'] = "dr-xr-x---"

    set_uid = d['name'].__contains__("_s_")
    if(set_uid):
        d['name'] = d['name'].replace("_s_", "")
        d['full_path'] = d["full_path"].replace("_s_", "")
        d['flags'] = "s" + d['flags'][1:]

    unreadable = d['name'].__contains__("_u_")
    if(unreadable):
        d['name'] = d['name'].replace("_u_", "")
        d['full_path'] = d["full_path"].replace("_u_", "")
        d['readable'] = False

    writable = d['name'].__contains__("_w_")
    if(writable):
        d['name'] = d['name'].replace("_w_", "")
        d['full_path'] = d["full_path"].replace("_w_", "")
        d['flags'] = d['flags'][0:2] + "w" + d['flags'][3:5] + "w" + d['flags'][6:8] + "w" + d['flags'][9:]

    if(d['name'].__contains__(".asset")):
        d['readable'] = False
        d['name'] = d['name'].lower().replace("command.asset", "")
        d['full_path'] = d['full_path'].lower().replace("command.asset", "")
        d['flags'] = "-r-xr-xr-x"
        d['content'] = ""

if __name__ == "__main__":
    for i in range(1, 5):
        main(str(i))