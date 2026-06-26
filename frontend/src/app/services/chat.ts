import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root',
})
export class Chat {
  private hubConnection!: signalR.HubConnection;
  public messageReceived$ = new Subject<{ user: string; message: string; time: string }>();
  public userJoined$ = new Subject<string>();
  public userLeft$ = new Subject<string>();
  public userCount$ = new Subject<number>();

  constructor() {}

  public startConnection(): void {
    const token = localStorage.getItem('auth_token');
    
    // Build connection; pass token in query if logged in
    const options: signalR.IHttpConnectionOptions = {
      skipNegotiation: false,
      transport: signalR.HttpTransportType.WebSockets
    };
    
    if (token) {
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_BASE_URL}/chathub?access_token=${token}`, options)
        .withAutomaticReconnect()
        .build();
    } else {
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_BASE_URL}/chathub`, options)
        .withAutomaticReconnect()
        .build();
    }

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established');
        this.registerHandlers();
      })
      .catch(err => console.error('Error starting SignalR connection: ', err));
  }

  private registerHandlers(): void {
    this.hubConnection.on('ReceiveMessage', (user: string, message: string, time: string) => {
      this.messageReceived$.next({ user, message, time });
    });

    this.hubConnection.on('UserJoined', (userName: string) => {
      this.userJoined$.next(userName);
    });

    this.hubConnection.on('UserLeft', (userName: string) => {
      this.userLeft$.next(userName);
    });

    this.hubConnection.on('UpdateUserCount', (count: number) => {
      this.userCount$.next(count);
    });
  }

  public joinChat(userName: string): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('JoinChat', userName)
        .catch(err => console.error('Error invoking JoinChat: ', err));
    }
  }

  public sendMessage(user: string, message: string): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('SendMessage', user, message)
        .catch(err => console.error('Error sending message: ', err));
    }
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}

