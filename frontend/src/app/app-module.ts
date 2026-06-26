import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Navbar } from './components/layout/navbar/navbar';
import { Footer } from './components/layout/footer/footer';
import { Home } from './components/public/home/home';
import { Blog } from './components/public/blog/blog';
import { BlogDetail } from './components/public/blog-detail/blog-detail';
import { Gallery } from './components/public/gallery/gallery';
import { ResumeDownload } from './components/public/resume-download/resume-download';
import { ChatWidget } from './components/public/chat-widget/chat-widget';
import { AdminLayout } from './components/admin/admin-layout/admin-layout';
import { Login } from './components/admin/login/login';
import { Dashboard } from './components/admin/dashboard/dashboard';
import { Profile } from './components/admin/profile/profile';
import { Skills } from './components/admin/skills/skills';
import { Projects } from './components/admin/projects/projects';
import { Experience } from './components/admin/experience/experience';
import { Education } from './components/admin/education/education';
import { Messages } from './components/admin/messages/messages';
import { BlogMgmt } from './components/admin/blog-mgmt/blog-mgmt';
import { BlogForm } from './components/admin/blog-form/blog-form';
import { GalleryMgmt } from './components/admin/gallery-mgmt/gallery-mgmt';
import { Logs } from './components/admin/logs/logs';
import { AuthInterceptor } from './services/auth-interceptor';

@NgModule({
  declarations: [
    App,
    Navbar,
    Footer,
    Home,
    Blog,
    BlogDetail,
    Gallery,
    ResumeDownload,
    ChatWidget,
    AdminLayout,
    Login,
    Dashboard,
    Profile,
    Skills,
    Projects,
    Experience,
    Education,
    Messages,
    BlogMgmt,
    BlogForm,
    GalleryMgmt,
    Logs
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [App]
})
export class AppModule { }
