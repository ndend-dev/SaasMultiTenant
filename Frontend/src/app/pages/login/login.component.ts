import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { AuthService } from '../../core/services/auth.service';
import { Workspace } from '../../core/models/workspace.model';
import { Router } from '@angular/router';
import { LoginResponse } from '../../core/models/loginResponse.model';
import { TokenRequest } from '../../core/models/tokenRequest.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    DialogModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private messageService = inject(MessageService);

  workspaces: Workspace[] = [];
  showWorkspaceDialog = signal<boolean>(false);
  loading = false;

  readonly loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        if (response) {
          this.showWorkspaceDialog.set(true);
          this.workspaces = this.authService.getWorkspaces;
        } else {
          this.showToast('Inicio de sesión fallido. Verifica tus credenciales.', 'error');
        }
      },
      error: (error) => {
        this.showToast('Error al intentar iniciar sesión.', 'error');
      },
    });
  }

  onSelectWorkspace(workspaceId: number): void {
    let tokenRequest: TokenRequest = {
      userId: this.authService.getUser?.id || 0,
      workspaceId: workspaceId,
    };

    this.authService.selectWorkspace(tokenRequest).subscribe({
      next: (success) => {
        if (success) {
          this.router.navigate(['/dashboard', workspaceId]);
        } else {
          this.showToast('Error al seleccionar el espacio de trabajo.', 'error');
        }
      },
      error: (error) => {
        this.showToast('Error al intentar seleccionar el espacio de trabajo.', 'error');
      },
    });
  }

  showToast(message: string, severity: 'success' | 'error' | 'info' | 'warn' = 'info'): void {
    this.messageService.add({ severity, summary: 'Notificación', detail: message });
  }
}
