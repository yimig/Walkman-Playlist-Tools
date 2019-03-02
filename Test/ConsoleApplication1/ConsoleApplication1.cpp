// ConsoleApplication1.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"
#include <iostream>


#define MAXSIZE 20
typedef struct {
	int r[MAXSIZE + 1];
	int length;
}SqList;//定义排序列表

//交换任意a和b的值
void Exchange(int &a, int &b)
{
	int c = a;
	a = b;
	b = c;
}

//确定枢轴，并且进行以枢轴为界对列表进行分割
int Partition(SqList &L, int low, int high)
{
	L.r[0] = L.r[low];//为监视哨赋值
	while (low < high)
	{
		while (low < high&&L.r[high] >= L.r[0])high--;
		Exchange(L.r[low], L.r[high]);
		while (low < high&&L.r[low] <= L.r[0])low++;
		Exchange(L.r[low], L.r[high]);
	}
	L.r[high] = L.r[0];
	return low;//返回分割点（枢轴）
}

//递归缩小列表的分割长度，直至排序完毕
void QSort(SqList &L, int low, int high)
{
	if (low < high)
	{
		int pivotloc = Partition(L, low, high);
		QSort(L, low, pivotloc - 1);
		QSort(L, pivotloc + 1, high);
	}
}

//对整个列表执行快速排序
void QuickSort(SqList &L)
{
	QSort(L, 1, L.length);
}

void main()
{
	SqList L;
	printf("请输入要排序的数组长度：（1~20）\n");
	scanf_s("%d", &L.length);
	printf("请依次输入要排序的数组元素：（用逗号隔开）\n");
	//数组的第一个元素为监视哨，应无实际值，所以这里要从1开始赋值
	for (int i = 1; i < L.length + 1; i++)scanf_s("%d,", &L.r[i]);
	QuickSort(L);
	printf("排序结果：\n");
	for (int i = 1; i < L.length + 1; i++)
	{
		printf("%d,", L.r[i]);
	}
	putchar('\n');
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门提示: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
