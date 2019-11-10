#define _CRT_SECURE_NO_WARNINGS 1 
#include <stdio.h>
#include "winsock2.h"
#include "ws2tcpip.h"
#include <ctime>
#include <iostream>
#include <chrono>

#pragma comment(lib, "Ws2_32.lib")
using namespace std;

void main() {

	WSADATA wsaData;
	SOCKET SendSocket;
	sockaddr_in RecvAddr;
	int Port = 13000;
	char RecvBuf[1024];
	char SendBuf[1024];
	int BufLen = 1024;
	int SenderAddrSize = sizeof(RecvAddr);

	WSAStartup(MAKEWORD(2, 2), &wsaData);
	
	SendSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	
	RecvAddr.sin_family = AF_INET;
	RecvAddr.sin_port = htons(Port);
	RecvAddr.sin_addr.s_addr;
	inet_pton(AF_INET, "127.0.0.1", &RecvAddr.sin_addr.s_addr);
	
	if (bind(SendSocket, (SOCKADDR*)&RecvAddr, sizeof(RecvAddr)) == SOCKET_ERROR)
	{
		printf("bind() failed.\n");
		closesocket(SendSocket);
		return;
	}
	
	printf("Receiving datagrams...\n");
	while (true) 
	{
		//recieve:
		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);

		for (int i = 100000; i > 0; --i) {
			char* Buffer = new char[i];

			printf("Sending a datagram to the receiver...\n");
			sendto(SendSocket, Buffer, sizeof(Buffer), 0, (SOCKADDR*)&RecvAddr, SenderAddrSize);
			delete Buffer;
		}
	}
	printf("Finished sending. Closing socket.\n");
	closesocket(SendSocket);
	
	printf("Exiting.\n");
	WSACleanup();
	return;
}