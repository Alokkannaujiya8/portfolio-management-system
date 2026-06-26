import { Component, OnInit } from '@angular/core';
import { Visitor } from '../../../services/visitor';
import { Resume } from '../../../services/resume';

@Component({
  selector: 'app-logs',
  standalone: false,
  templateUrl: './logs.html',
  styleUrl: './logs.css',
})
export class Logs implements OnInit {
  visitorLogs: any[] = [];
  downloadLogs: any[] = [];
  
  isLoading = true;
  activeTab = 'visitors'; // 'visitors' | 'downloads'
  
  searchTerm = '';

  constructor(
    private readonly visitorService: Visitor,
    private readonly resumeService: Resume
  ) {}

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.isLoading = true;
    if (this.activeTab === 'visitors') {
      this.visitorService.getLogs().subscribe({
        next: (data) => {
          // Sort descending by first visit or created date
          this.visitorLogs = data.sort((a, b) => new Date(b.createdDate || b.firstVisit).getTime() - new Date(a.createdDate || a.firstVisit).getTime());
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching visitor logs', err);
          this.isLoading = false;
        }
      });
    } else {
      this.resumeService.getLogs().subscribe({
        next: (data) => {
          // Sort descending by viewDate or createdDate
          this.downloadLogs = data.sort((a, b) => new Date(b.createdDate || b.viewDate).getTime() - new Date(a.createdDate || a.viewDate).getTime());
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching download logs', err);
          this.isLoading = false;
        }
      });
    }
  }

  switchTab(tab: string): void {
    this.activeTab = tab;
    this.searchTerm = '';
    this.loadLogs();
  }

  getFilteredVisitorLogs() {
    if (!this.searchTerm.trim()) return this.visitorLogs;
    const term = this.searchTerm.toLowerCase().trim();
    return this.visitorLogs.filter(log => 
      (log.ipAddress && log.ipAddress.toLowerCase().includes(term)) ||
      (log.country && log.country.toLowerCase().includes(term)) ||
      (log.city && log.city.toLowerCase().includes(term)) ||
      (log.isp && log.isp.toLowerCase().includes(term))
    );
  }

  getFilteredDownloadLogs() {
    if (!this.searchTerm.trim()) return this.downloadLogs;
    const term = this.searchTerm.toLowerCase().trim();
    return this.downloadLogs.filter(log => 
      (log.visitorName && log.visitorName.toLowerCase().includes(term)) ||
      (log.visitorEmail && log.visitorEmail.toLowerCase().includes(term)) ||
      (log.companyName && log.companyName.toLowerCase().includes(term)) ||
      (log.designation && log.designation.toLowerCase().includes(term)) ||
      (log.ipAddress && log.ipAddress.toLowerCase().includes(term)) ||
      (log.country && log.country.toLowerCase().includes(term)) ||
      (log.city && log.city.toLowerCase().includes(term))
    );
  }
}
