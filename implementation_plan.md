# Migration Plan: Portfolio Management System MVC to Angular + Web API

We will implement all missing frontend Angular components and services to interface with the already completed ASP.NET Core Web API backend. This will make the modern decoupled application fully functional and parity-compliant with the original monolithic MVC project.

## Proposed Changes

We will implement the Angular frontend components (both public pages and admin dashboard controls) and their supporting services, and configure the authentication flow.

### 🌐 Shared & Core Services

#### [MODIFY] [auth.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/auth.ts)
* Implement JWT login storage, token decoding (to check roles/expirations), validation, and logout.

#### [NEW] [auth-interceptor.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/auth-interceptor.ts)
* Create an Angular HTTP Interceptor to automatically attach the `Authorization: Bearer <token>` header to all outgoing requests.

#### [MODIFY] [app-module.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/app-module.ts)
* Import `FormsModule` and register the `AuthInterceptor` provider.

#### [MODIFY] [auth-guard.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/guards/auth-guard.ts)
* Update guard to read the login status from `AuthService` and redirect unauthenticated users to `/admin/login`.

#### [MODIFY] [portfolio.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/portfolio.ts)
* Add CRUD methods for Profile, Skills, Projects, Experience, and Education to allow admin mutations.

---

### 📝 Features & Services (Public Section)

#### [MODIFY] [blog.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/blog.ts)
* Add calls for `GET posts` (with filters/pagination), `GET posts/:slug`, `GET categories`, `POST posts/:id/like`, and `POST posts/:id/comment`.

#### [MODIFY] [gallery.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/gallery.ts)
* Add calls for getting gallery items, albums, and album details.

#### [MODIFY] [resume.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/resume.ts)
* Add calls to track resume downloads, get location info, and trigger download streams.

#### [MODIFY] [visitor.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/visitor.ts)
* Track site visits on route changes and read dashboard stats for admin.

#### [MODIFY] [chat.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/services/chat.ts)
* Implement SignalR client hub connection to `/chathub` for real-time messaging.

#### [MODIFY] [blog.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/blog/blog.ts) & [blog.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/blog/blog.html)
* Render blog posts feed with search, category filtering, and tags.

#### [MODIFY] [blog-detail.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/blog-detail/blog-detail.ts) & [blog-detail.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/blog-detail/blog-detail.html)
* Render full blog content, feature images, likes count toggles, and dynamic comments (including nested replies).

#### [MODIFY] [gallery.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/gallery/gallery.ts) & [gallery.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/gallery/gallery.html)
* Render photo galleries grouped by albums, including lightboxes and view counts.

#### [MODIFY] [resume-download.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/resume-download/resume-download.ts) & [resume-download.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/resume-download/resume-download.html)
* Implement the MVC-style resume form, fetching visitor geo-location, calling the `track` endpoint, and initiating the PDF file download.

#### [MODIFY] [chat-widget.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/chat-widget/chat-widget.ts) & [chat-widget.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/public/chat-widget/chat-widget.html)
* A floating chat box connecting to the SignalR Hub, allowing visitors to chat with the admin in real-time.

---

### 🛠️ Admin Dashboard Section (`admin/`)

#### [MODIFY] [login.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/login/login.ts) & [login.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/login/login.html)
* Admin sign-in interface communicating with `api/Auth/login` and storing authorization tokens.

#### [MODIFY] [dashboard.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/dashboard/dashboard.ts) & [dashboard.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/dashboard/dashboard.html)
* Render statistics graphs (visitor logs, download logs) and counts.

#### [MODIFY] [profile.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/profile/profile.ts) & [profile.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/profile/profile.html)
* Profile management forms supporting Photo/Resume file upload.

#### [MODIFY] [skills.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/skills/skills.ts) & [skills.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/skills/skills.html)
* CRUD interface for managing technical skills (SkillName and Percentage bar).

#### [MODIFY] [projects.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/projects/projects.ts) & [projects.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/projects/projects.html)
* CRUD interface for projects including image uploads, live links, and descriptions.

#### [MODIFY] [experience.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/experience/experience.ts) & [experience.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/experience/experience.html)
* CRUD interface for work experiences (Company, Role, Start/End date, Description).

#### [MODIFY] [education.ts (Component)](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/education/education.ts) & [education.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/education/education.html)
* CRUD interface for educations (Degree, Institute, Year, Percentage).

#### [MODIFY] [messages.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/messages/messages.ts) & [messages.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/messages/messages.html)
* Message inbox. Display contact submissions, mark as read, and write email replies.

#### [MODIFY] [blog-mgmt.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/blog-mgmt/blog-mgmt.ts) & [blog-mgmt.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/blog-mgmt/blog-mgmt.html)
* List all blogs (CRUD trigger) and manage categories (Add/Delete categories).

#### [MODIFY] [blog-form.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/blog-form/blog-form.ts) & [blog-form.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/blog-form/blog-form.html)
* Add/Edit blog posts form supporting multipart data uploads (featured image, post videos, tags, categories).

#### [MODIFY] [gallery-mgmt.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/gallery-mgmt/gallery-mgmt.ts) & [gallery-mgmt.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/gallery-mgmt/gallery-mgmt.html)
* Album creation, photo and video upload tools, and item sorting controls.

#### [MODIFY] [logs.ts](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/logs/logs.ts) & [logs.html](file:///d:/Asp.net%20core_Project/Portfolio%20Management%20Systembyalok/asp.netcore/frontend/src/app/components/admin/logs/logs.html)
* Display analytical records (visitor tracking and resume downloads).

---

## Verification Plan

### Manual Verification
1. Build both projects (`dotnet build` for backend, `npm run build` for frontend) to guarantee compile-time correctness.
2. Confirm the Angular SPA integrates seamlessly with backend endpoints.
