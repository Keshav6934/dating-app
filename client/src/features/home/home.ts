import { Component, inject, OnInit, signal } from '@angular/core';
import { Register } from "../account/register/register";
import { AccountService } from '../../core/services/account-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  protected registerMode = signal(false);
  protected accountService = inject(AccountService);
  private router = inject(Router);

  ngOnInit(): void {
    if (this.accountService.currentUser()) {
      this.router.navigateByUrl('/members');
    }
  }

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }
}