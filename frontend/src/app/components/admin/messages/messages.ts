import { Component, OnInit } from '@angular/core';
import { Portfolio } from '../../../services/portfolio';

@Component({
  selector: 'app-messages',
  standalone: false,
  templateUrl: './messages.html',
  styleUrl: './messages.css',
})
export class Messages implements OnInit {
  messages: any[] = [];
  isLoading = true;
  selectedMessage: any = null;
  replyText = '';

  constructor(private readonly portfolioService: Portfolio) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.isLoading = true;
    this.portfolioService.getMessages().subscribe({
      next: (data) => {
        // Sort descending by created date
        this.messages = data.sort((a: any, b: any) => new Date(b.createdDate || b.createdDate).getTime() - new Date(a.createdDate || a.createdDate).getTime());
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching messages', err);
        this.isLoading = false;
      }
    });
  }

  viewMessage(msg: any): void {
    this.selectedMessage = msg;
    this.replyText = `Hi ${msg.name},\n\nThank you for contacting me. \n\nBest regards,\n`;
    
    if (!msg.isRead) {
      this.portfolioService.markMessageAsRead(msg.messageId).subscribe({
        next: () => {
          msg.isRead = true;
        },
        error: (err) => console.error('Error marking message as read', err)
      });
    }
  }

  sendEmailReply(): void {
    if (!this.selectedMessage || !this.replyText) return;
    
    const email = this.selectedMessage.email;
    const subject = encodeURIComponent('Re: ' + (this.selectedMessage.subject || 'Your message'));
    const body = encodeURIComponent(this.replyText);
    
    window.location.href = `mailto:${email}?subject=${subject}&body=${body}`;
  }

  closeModal(): void {
    this.selectedMessage = null;
  }
}
