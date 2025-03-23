import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const token = localStorage.getItem('token');
    if (token) {
      // Dacă există token-ul, utilizatorul poate accesa pagina
      return true;
    } else {
      // Dacă nu există token-ul, utilizatorul este redirecționat la login
      this.router.navigate(['/login']);
      return false;
    }
  }
}
