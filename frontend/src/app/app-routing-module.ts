import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayout } from './components/admin/admin-layout/admin-layout';
import { Blog } from './components/public/blog/blog';
import { BlogDetail } from './components/public/blog-detail/blog-detail';
import { BlogForm } from './components/admin/blog-form/blog-form';
import { BlogMgmt } from './components/admin/blog-mgmt/blog-mgmt';
import { Dashboard } from './components/admin/dashboard/dashboard';
import { Education } from './components/admin/education/education';
import { Experience } from './components/admin/experience/experience';
import { Gallery } from './components/public/gallery/gallery';
import { GalleryMgmt } from './components/admin/gallery-mgmt/gallery-mgmt';
import { Home } from './components/public/home/home';
import { Login } from './components/admin/login/login';
import { Logs } from './components/admin/logs/logs';
import { Messages } from './components/admin/messages/messages';
import { Profile } from './components/admin/profile/profile';
import { Projects } from './components/admin/projects/projects';
import { ResumeDownload } from './components/public/resume-download/resume-download';
import { Skills } from './components/admin/skills/skills';
import { authGuard } from './guards/auth-guard';

const routes: Routes = [
  { path: '', component: Home, pathMatch: 'full' },
  { path: 'blog', component: Blog },
  { path: 'blog/:slug', component: BlogDetail },
  { path: 'gallery', component: Gallery },
  { path: 'resume', component: ResumeDownload },
  { path: 'admin/login', component: Login },
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: Dashboard },
      { path: 'profile', component: Profile },
      { path: 'skills', component: Skills },
      { path: 'projects', component: Projects },
      { path: 'experience', component: Experience },
      { path: 'education', component: Education },
      { path: 'messages', component: Messages },
      { path: 'blog', component: BlogMgmt },
      { path: 'blog/new', component: BlogForm },
      { path: 'blog/edit/:id', component: BlogForm },
      { path: 'gallery', component: GalleryMgmt },
      { path: 'logs', component: Logs }
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
