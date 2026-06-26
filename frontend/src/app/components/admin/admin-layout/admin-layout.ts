import { Component } from '@angular/core';

import { Auth } from '../../../services/auth';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css',
})
export class AdminLayout {
  constructor(private readonly auth: Auth) {}

  onLogout(): void {
    this.auth.logout();
  }
}

