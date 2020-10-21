#include <stdlib.h>
#include <sys/types.h>
#include <unistd.h>
 
int main(void)
{
    setreuid(geteuid(), geteuid());
    system("cat /home/root/welcome_message.txt");
    return 0;
}