import { AfterViewInit, Component } from '@angular/core';
import { HomeData, Portfolio } from '../../../services/portfolio';

declare const AOS: { init: (options: object) => void };
declare const Typed: new (selector: string, options: object) => object;

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements AfterViewInit {
  data?: HomeData;
  isLoading = true;
  errorMessage = '';

  constructor(private readonly portfolio: Portfolio) {
    this.portfolio.getHomeData().subscribe({
      next: data => {
        this.data = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Backend API se data load nahi ho pa raha. API run karke connection string check karein.';
        this.isLoading = false;
      }
    });
  }

  assetUrl(path?: string): string {
    if (!path) {
      return '';
    }

    return path.startsWith('http') ? path : `http://localhost:5154${path}`;
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (typeof AOS !== 'undefined') {
        AOS.init({ duration: 1000, once: true });
      }

      if (typeof Typed !== 'undefined') {
        new Typed('#typing-text', {
          strings: [
            this.data?.profile?.title || 'Full Stack Developer | ASP.NET Core | SQL Server | AI/ML',
            '.NET Full Stack Developer & AI/ML Engineer',
            'Full Stack Developer (ASP.NET Core) | Machine Learning Enthusiast',
            'Problem Solver',
            'Creative Thinker'
          ],
          typeSpeed: 50,
          backSpeed: 30,
          loop: true
        });
      }
    }, 300);
  }

  shortText(value?: string, length = 100): string {
    if (!value) {
      return '';
    }

    return value.length > length ? `${value.substring(0, length)}...` : value;
  }

  totalExperienceYears(): number {
    const totalMonths = this.data?.experience?.reduce((sum, item) => {
      const start = new Date(item.startDate);
      const end = item.endDate ? new Date(item.endDate) : new Date();
      const months = Math.max(0, (end.getFullYear() - start.getFullYear()) * 12 + end.getMonth() - start.getMonth());
      return sum + months;
    }, 0) ?? 0;

    return Math.max(0, Math.floor(totalMonths / 12));
  }

  contactModel = { name: '', email: '', subject: '', message: '' };
  isSubmitting = false;
  contactSuccess = '';
  contactError = '';

  onSendMessage(): void {
    this.isSubmitting = true;
    this.contactSuccess = '';
    this.contactError = '';
    this.portfolio.sendMessage(this.contactModel).subscribe({
      next: (res) => {
        this.contactSuccess = 'Thank you! Your message has been sent successfully.';
        this.contactModel = { name: '', email: '', subject: '', message: '' };
        this.isSubmitting = false;
      },
      error: (err) => {
        this.contactError = err.error?.message || 'Failed to send message. Please try again.';
        this.isSubmitting = false;
      }
    });
  }
}
