import { afterNextRender, Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../core/services/project.service';

import { DataViewModule } from 'primeng/dataview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';
import { User } from '../../core/models/user.model';
import { ProjectResponse } from '../../core/models/projectResponse.model';

@Component({
  selector: 'app-dashboard.component',
  imports: [CommonModule, DataViewModule, ButtonModule, TagModule, AvatarModule, TooltipModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  public readonly authService = inject(AuthService);
  public readonly projectService = inject(ProjectService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  workspaceId: number | null = null;
  user: User | null = null;
  public readonly loading = signal<boolean>(true);
  projects = signal<ProjectResponse[]>([]);

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

  createProject(): void {
    // Implement project creation logic here

    console.log('Create project clicked 0' + this.authService.getUser?.firstName);
  }

}
