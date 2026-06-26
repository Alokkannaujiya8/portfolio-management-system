import { Component, OnInit } from '@angular/core';
import { Portfolio, Profile } from '../../../services/portfolio';

@Component({
  selector: 'app-footer',
  standalone: false,
  templateUrl: './footer.html',
  styleUrl: './footer.css',
})
export class Footer implements OnInit {
  profile?: Profile;
  currentYear = new Date().getFullYear();
  isDarkMode = false;

  constructor(private readonly portfolio: Portfolio) {
    this.portfolio.getProfile().subscribe({
      next: profile => this.profile = profile,
      error: () => this.profile = undefined
    });
  }

  ngOnInit(): void {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
      this.isDarkMode = true;
      document.body.classList.add('dark-theme');
    }
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      document.body.classList.add('dark-theme');
      localStorage.setItem('theme', 'dark');
    } else {
      document.body.classList.remove('dark-theme');
      localStorage.setItem('theme', 'light');
    }
  }

  shortDescription(): string {
    const description = this.profile?.description || 'Creating amazing digital experiences with passion and creativity.';
    return description.length > 100 ? `${description.substring(0, 100)}...` : description;
  }
}
