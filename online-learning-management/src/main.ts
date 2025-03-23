import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app/app.routes';
import { authInterceptor } from './app/services/auth.interceptor';

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    provideRouter(routes), // Configurarea rutelor
    provideHttpClient(withInterceptors([authInterceptor])), // AdaugÄ interceptorul funcČ›ional
  ],
}).catch((err) => {
  console.error(err);
});
