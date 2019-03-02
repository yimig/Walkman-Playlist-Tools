
#include<stdio.h>

int i = 5;
void mian()
{
	int fun1(void);
	int i = 3;
	{
		int i = 10;
		i++;
	}
	fun1();
	i++;
	printf("%d\n", i);
}

int fun1(void)
{
	i++;
	return (i);
}