#define _CRT_SECURE_NO_WARNINGS 1 
#include <stdio.h>
#include "winsock2.h"
#include "ws2tcpip.h"
#include <ctime>
#include <iostream>
#include <chrono>
#include <fstream>
#include <string>

#pragma comment(lib, "Ws2_32.lib")
using namespace std;
void main()
{
	// visszatéritési érték tárolásához
	int iResult;
	//----------------------
	// Inicializálja a Winsock könyvtárat
	WSADATA wsaData;
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != NO_ERROR) {
		printf("WSAStartup() error!\n");
	}
	//----------------------
	// Létrehozunk egy SOCKET-et amelyik a szerverhez kapcsolódik

	SOCKET ListenSocket;
	ListenSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (ListenSocket == INVALID_SOCKET)
	{
		printf("Error on initializing socket. Error code: %ld\n",
			WSAGetLastError());
		// WSACleanup befejezi a Windows Sockets működését minden szálban!!!!
		WSACleanup();
		return;
	}
	//----------------------
	// A sockaddr_in struktúra meghatározza a cím családot,
	// Az IP címet és a portot amihez kapcslódnia kell

	int Port = 2269;
	char IP[10] = "127.0.0.1";
	sockaddr_in ServerAddr;
	int AddrLen = sizeof(ServerAddr);
	ServerAddr.sin_family = AF_INET;
	ServerAddr.sin_addr.s_addr;
	inet_pton(AF_INET, IP, &ServerAddr.sin_addr.s_addr);
	ServerAddr.sin_port = htons(Port);
	//----------------------
	if (bind(ListenSocket, (SOCKADDR*)&ServerAddr, AddrLen) < 0)
	{
		printf("Error on binding socket to address.%ld\n", WSAGetLastError());
		WSACleanup();
		return;
	}
	//------------------------------------------------------
	if (listen(ListenSocket, 1) == SOCKET_ERROR) {
		printf("Error on listening on socket.\n");
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}
	//-------------------------------------------------------
	// Letrehozunk egy socketet ami fogadja a klienst
	SOCKET AcceptSocket;

	//-----------------------------------------
	const int recvBufLen = 1024;
	char RecvBuf[recvBufLen];
	/*recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
	if ((int)RecvBuf) {
		RecvAddr1 = RecvAddr;
		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
		RecvAddr2 = RecvAddr;
	}
	else {
		RecvAddr2 = RecvAddr;
		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
		RecvAddr1 = RecvAddr;
	}*/
	while (true) {
		printf("Waiting for client to connect.\n");
		//-----------------------------------------
		if ((AcceptSocket = accept(ListenSocket, NULL, NULL)) == INVALID_SOCKET) {
			printf("Accept failed:Invalid socket.%ld\n", WSAGetLastError());
			closesocket(ListenSocket);
			WSACleanup();
			return;
		}
		else {
			printf("Accept succesful:Client connected!\n");
		}
		recv(AcceptSocket, RecvBuf, recvBufLen, 0);
		//x = (int)RecvBuf;
		cout << RecvBuf;
		char message[] = "uzenetet kuldtem";
		send(AcceptSocket, message, sizeof(message), 0);

		//fout << x << " " << y << endl;
		printf("Sending a datagram to the receiver...\n");
		//send(SendSocket, sizeof(Buffer), 0);

		//iResult = recv(AcceptSocket, RecBuf, recvBufLen - 1, 0);
		//RecBuf[iResult] = '\0';
		//printf("Client sent: %s\n", RecBuf);
		//if (iResult == SOCKET_ERROR)
		//{
		//	printf("Error on receiving data. Error code: %d\n",
		//		WSAGetLastError());
		//	closesocket(AcceptSocket);
		//	WSACleanup();
		//	return;
		//}
		////ha lezártuk valhol a socketet
		//if (iResult == 0)
		//{
		//	return;
		//}
		//printf("Sending a datagram...\n");
		//send(AcceptSocket, RecBuf, recvBufLen - 1, 0);
		//printf("Finished sending. Closing socket.\n");
		//iResult = shutdown(AcceptSocket, SD_SEND);
	}
	WSACleanup();
	return;
}
//void main() {
//	WSADATA wsaData;
//	SOCKET SendSocket;
//	sockaddr_in RecvAddr;
//	sockaddr_in RecvAddr1;
//	sockaddr_in RecvAddr2;
//	int Port = 13000;
//	char RecvBuf[1024];
//	char SendBuf[1024];
//	int BufLen = 1024;
//	int SenderAddrSize = sizeof(RecvAddr);
//	int x, y;
//	ofstream fout;
//	fout.open("Gamelog.txt");
//
//	//Kliens listat kesziteni, ami tartalmazza az osszes csatlakoztatott klienst
//	WSAStartup(MAKEWORD(2, 2), &wsaData);
//	
//	SendSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
//	
//	RecvAddr.sin_family = AF_INET;
//	RecvAddr.sin_port = htons(Port);
//	RecvAddr.sin_addr.s_addr;
//	inet_pton(AF_INET, "127.0.0.1", &RecvAddr.sin_addr.s_addr);
//	
//	if (bind(SendSocket, (SOCKADDR*)&RecvAddr, sizeof(RecvAddr)) == SOCKET_ERROR)
//	{
//		printf("bind() failed.\n");
//		closesocket(SendSocket);
//		return;
//	}
//	/*recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
//	if ((int)RecvBuf) {
//		RecvAddr1 = RecvAddr;
//		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
//		RecvAddr2 = RecvAddr;
//	}
//	else {
//		RecvAddr2 = RecvAddr;
//		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
//		RecvAddr1 = RecvAddr;
//	}*/
//	printf("Receiving datagrams...\n");
//	while (true) 
//	{
//		//recieve:
//		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
//		x = (int)RecvBuf;
//		recvfrom(SendSocket, RecvBuf, BufLen, 0, (SOCKADDR*)&RecvAddr, &SenderAddrSize);
//		y= (int)RecvBuf;
//		fout<<x<<" "<<y<<endl;
//		printf("Sending a datagram to the receiver...\n");
//		sendto(SendSocket, , sizeof(Buffer), 0, (SOCKADDR*)&RecvAddr, SenderAddrSize);
//		
//	}
//	printf("Finished sending. Closing socket.\n");
//	closesocket(SendSocket);
//	
//	printf("Exiting.\n");
//	WSACleanup();
//	return;
//}
