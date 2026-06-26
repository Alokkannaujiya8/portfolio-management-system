import { Component, OnInit, signal } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Visitor } from './services/visitor';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('frontend');

  constructor(
    private readonly router: Router,
    private readonly visitorService: Visitor
  ) {}

  ngOnInit(): void {
    this.initializeTracking();
    this.trackNavigation();
  }

  private initializeTracking(): void {
    let visitorId = localStorage.getItem('visitor_id');
    let sessionId = localStorage.getItem('session_id');

    if (!sessionId) {
      sessionId = this.generateGuid();
      localStorage.setItem('session_id', sessionId);
    }

    // Call API to register/track visitor session
    const trackingObj = {
      visitorId: visitorId ? parseInt(visitorId) : 0,
      sessionId: sessionId,
      visitCount: 1,
      isActive: true
    };

    this.visitorService.trackVisitor(trackingObj).subscribe({
      next: (res) => {
        if (res && res.visitorId) {
          localStorage.setItem('visitor_id', res.visitorId.toString());
        }
      },
      error: (err) => console.log('Visitor tracking failed', err)
    });
  }

  private trackNavigation(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: (event: any) => {
        const visitorId = localStorage.getItem('visitor_id');
        if (visitorId) {
          const pageTitle = this.getPageTitle(event.urlAfterRedirects);
          const pageVisit = {
            visitorId: parseInt(visitorId),
            pageUrl: event.urlAfterRedirects,
            pageTitle: pageTitle,
            visitTime: new Date().toISOString(),
            isActive: true
          };

          this.visitorService.trackPageVisit(pageVisit).subscribe({
            error: (err) => console.log('Page visit tracking failed', err)
          });
        }
      }
    });
  }

  private getPageTitle(url: string): string {
    if (url === '/') return 'Home';
    if (url.startsWith('/blog')) return 'Blog';
    if (url.startsWith('/gallery')) return 'Gallery';
    if (url.startsWith('/resume')) return 'Resume';
    if (url.startsWith('/admin')) return 'Admin Dashboard';
    return 'Portfolio';
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}

