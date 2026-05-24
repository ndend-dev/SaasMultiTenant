import { afterNextRender, Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../core/services/project.service';
import { CommonModule } from '@angular/common';
import { User } from '../../core/models/user.model';
import { ProjectResponse } from '../../core/models/projectResponse.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { DataViewModule } from 'primeng/dataview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-dashboard.component',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DataViewModule,
    ButtonModule,
    TagModule,
    AvatarModule,
    InputTextModule, 
    TextareaModule,
    TooltipModule,
    DialogModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  public readonly authService = inject(AuthService);
  public readonly projectService = inject(ProjectService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);
  private messageService = inject(MessageService);

  workspaceId: number | null = null;
  user: User | null = null;
  public readonly loading = signal<boolean>(true);
  projects = signal<ProjectResponse[]>([]);

  public displayCreateDialog = signal(false);


  readonly projectForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: ['', [Validators.required, Validators.maxLength(200)]]
  });

  constructor() {
    afterNextRender(() => {
      this.workspaceId = this.route.snapshot.paramMap.get('workspaceId')
        ? +this.route.snapshot.paramMap.get('workspaceId')!
        : null;

      if (!this.authService.isAuthenticated$ || !this.authService.getTokenInfo()) {
        this.authService.logout();
        this.router.navigate(['/login']);
      }

      if (this.workspaceId) {
        this.getProjects();
      } else {
        console.error('Workspace ID is missing in the route parameters');
      }
    });
  }


  getProjects(): void {
    this.projectService.getProjects(this.workspaceId || 0).subscribe((projects) => {
      console.log('Projects fetched:', projects);
      this.projects.set(projects);
      this.loading.set(false);
    });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  showCreateDialog(): void {
    this.displayCreateDialog.set(true);
  }


  createProject(): void {
    if (this.projectForm.invalid) {
      this.projectForm.markAllAsTouched();
      return;
    }

    const projectRequest = {
      workspaceId: this.workspaceId || 0,
      name: this.projectForm.value.name,
      description: this.projectForm.value.description,
    };

    this.projectService.createProject(projectRequest).subscribe({
      next: (success) => {
        if (success) {
          this.getProjects();
          this.displayCreateDialog.set(false);
          this.projectForm.reset();
          this.showToast('Proyecto creado exitosamente.', 'success');
        }
      },
      error: (err) => {
        console.error('Error creating project:', err);
        this.showToast('Error al crear el proyecto.', 'error');
      }
    });
  }

  showToast(message: string, severity: 'success' | 'error' | 'info' | 'warn' = 'info'): void {
    this.messageService.add({ severity, summary: 'Notificación', detail: message });
  }
}
