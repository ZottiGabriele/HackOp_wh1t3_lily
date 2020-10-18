import sys
import os
import json

#/mnt/c/Users/Zotti/Documents/0_UNITY/HackOp_wh1t3_lily/Assets/Resources

def main(ch_number: str):
    vfs_path = "C:\\Users\\Zotti\\Documents\\0_UNITY\\HackOp_wh1t3_lily\\Assets\\Resources"
    start_path = vfs_path + "\\VFS\\challenge_" + ch_number + "\\"
    json_path = vfs_path + "\\VFS\\vfs_ch_" + ch_number + ".json" 
    d = {'contents':path_to_dict("\\".join(start_path.split("\\")[:-1]), start_path, vfs_path)}
    out_file = open(json_path, "w")
    out = json.dumps(d, indent=2)
    out_file.write(out)
    out_file.close()
    print("\n\nOutput json written at " + json_path + "\n\n")
    
def path_to_dict(start_path, path, vfs_path):
    file_name = os.path.basename(path)

    #if it has more than one extension, remove the last one
    if(len(file_name.split(".")) > 2):
        file_name = ".".join(file_name.split('.')[:-1])

    d = {'hidden': file_name.__contains__("_h_")}
    d['name'] = file_name if not d['hidden'] else file_name.replace("_h_", ".")
    d['full_path'] = path[len(start_path):len(path)-len(os.path.basename(path))].replace("\\","/").replace("_h_", ".") + d['name'] + "/"
    d['r_full_path'], _ = os.path.splitext(path[len(vfs_path) + 1:].replace("\\","/"))
    d['flags'] = "drwxr-xr-x"
    d['user'] = "user" if not file_name.__contains__("_r_") else "root"
    d['group'] = "group" if not file_name.__contains__("_r_") else "root"

    #fix root folder
    if(file_name == ""):
        d['name'] = "/"
        d['full_path'] = "/"
    
    if os.path.isdir(path):
        d['type'] = "directory"
        d['contents'] = []
        for x in os.listdir(path):
            #skip .meta files
            if(not x.__contains__(".meta")):
                d['contents'].append(path_to_dict(start_path, os.path.join(path,x), vfs_path))
    else:
        d['type'] = "file"
        d['flags'] = "-rwxr-xr-x"
        if(file_name.__contains__("_s_")):
            d['flags'] = "srwxr-xr-x"

    clean(d)

    return d

def clean(d):
    d['name'] = d['name'].replace("_r_", "")
    d['name'] = d['name'].replace("_s_", "")

    d['full_path'] = d["full_path"].replace("_r_", "")
    d['full_path'] = d["full_path"].replace("_s_", "")

if __name__ == "__main__":
    for i in range(1, 5):
        main(str(i))