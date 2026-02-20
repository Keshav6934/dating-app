import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { catchError, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountService = inject(AccountService);

  init() {

    const userString = localStorage.getItem('user');
    
    if (userString) {
      const user = JSON.parse(userString);
      this.accountService.setCurrentUser(user);
    }


    return this.accountService.refreshToken().pipe(
      tap(user => {
        if (user) {
          this.accountService.setCurrentUser(user);
          this.accountService.startTokenRefreshInterval();
        }
      }),

      catchError(error => {
        this.accountService.logout();
        return of(null);
      })
    )
  }
}