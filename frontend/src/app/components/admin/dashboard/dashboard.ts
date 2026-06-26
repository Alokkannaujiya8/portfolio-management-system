import { Component, OnInit } from '@angular/core';
import { Visitor } from '../../../services/visitor';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  stats: any = {
    projectsCount: 0,
    skillsCount: 0,
    messagesCount: 0,
    blogsCount: 0,
    resumeViewsCount: 0,
    recentVisitorLogs: [],
    recentResumeDownloads: []
  };
  isLoading = true;

  constructor(private readonly visitorService: Visitor) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.isLoading = true;
    this.visitorService.getDashboardStats().subscribe({
      next: (data) => {
        this.stats = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching dashboard statistics', err);
        this.isLoading = false;
      }
    });
  }
}

