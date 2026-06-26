import { Component } from '@angular/core';
import { Portfolio, Profile } from '../../../services/portfolio';
import { Auth } from '../../../services/auth';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  profile?: Profile;

  constructor(
    private readonly portfolio: Portfolio,
    private readonly auth: Auth
  ) {
    this.portfolio.getProfile().subscribe({
      next: profile => this.profile = profile,
      error: () => this.profile = undefined
    });
  }

  isLoggedIn(): boolean {
    return this.auth.isAuthenticated();
  }

  onLogout(): void {
    this.auth.logout();
  }
}
