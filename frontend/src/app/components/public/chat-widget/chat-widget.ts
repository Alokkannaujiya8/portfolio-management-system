import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { Chat } from '../../../services/chat';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-chat-widget',
  standalone: false,
  templateUrl: './chat-widget.html',
  styleUrl: './chat-widget.css',
})
export class ChatWidget implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('scrollContainer') private readonly scrollContainer!: ElementRef;

  isOpen = false;
  joined = false;
  userName = '';
  messageText = '';
  
  messages: Array<{ user: string; message: string; time: string; isSelf: boolean }> = [];
  userCount = 0;

  private messageSub!: Subscription;
  private joinSub!: Subscription;
  private leaveSub!: Subscription;
  private countSub!: Subscription;

  constructor(private readonly chatService: Chat) {}

  ngOnInit(): void {
    // Read saved username if any
    const savedName = localStorage.getItem('chat_username');
    if (savedName) {
      this.userName = savedName;
      this.joined = true;
    }

    this.chatService.startConnection();

    // Subscribe to SignalR events
    this.messageSub = this.chatService.messageReceived$.subscribe({
      next: (msg) => {
        this.messages.push({
          user: msg.user,
          message: msg.message,
          time: msg.time,
          isSelf: msg.user === this.userName
        });
      }
    });

    this.joinSub = this.chatService.userJoined$.subscribe({
      next: (user) => {
        this.messages.push({
          user: 'System',
          message: `${user} joined the chat`,
          time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
          isSelf: false
        });
      }
    });

    this.leaveSub = this.chatService.userLeft$.subscribe({
      next: (user) => {
        this.messages.push({
          user: 'System',
          message: `${user} left the chat`,
          time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
          isSelf: false
        });
      }
    });

    this.countSub = this.chatService.userCount$.subscribe({
      next: (count) => this.userCount = count
    });
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  ngOnDestroy(): void {
    if (this.messageSub) this.messageSub.unsubscribe();
    if (this.joinSub) this.joinSub.unsubscribe();
    if (this.leaveSub) this.leaveSub.unsubscribe();
    if (this.countSub) this.countSub.unsubscribe();
    this.chatService.stopConnection();
  }

  toggleChat(): void {
    this.isOpen = !this.isOpen;
    if (this.isOpen && this.joined && this.userName) {
      this.chatService.joinChat(this.userName);
    }
  }

  joinChat(): void {
    if (this.userName.trim()) {
      localStorage.setItem('chat_username', this.userName.trim());
      this.joined = true;
      this.chatService.joinChat(this.userName);
    }
  }

  sendMessage(): void {
    if (this.messageText.trim() && this.userName) {
      this.chatService.sendMessage(this.userName, this.messageText.trim());
      this.messageText = '';
    }
  }

  private scrollToBottom(): void {
    try {
      if (this.scrollContainer) {
        this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
      }
    } catch (err) {}
  }
}


