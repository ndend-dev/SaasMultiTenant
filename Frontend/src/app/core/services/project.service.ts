import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProjectResponse } from '../models/projectResponse.model';
import { isPlatformBrowser } from '@angular/common';
import { ProjectRequest } from '../models/projectRequest.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly apiUrl = environment.apiUrl;
  private readonly apiRoute = 'projects';

  private readonly projectsSubject = new BehaviorSubject<ProjectResponse[]>([]);
  readonly projects$ = this.projectsSubject.asObservable();

  private getAuthHeaders(): HttpHeaders {
    const token = isPlatformBrowser(this.platformId) ? localStorage.getItem('auth_token') : null;
    return token ? new HttpHeaders({ Authorization: `Bearer ${token}` }) : new HttpHeaders();
  }

  getProjects(workspaceId: number): Observable<ProjectResponse[]> {
    const headers = this.getAuthHeaders();
    return this.http
      .get<ProjectResponse[]>(`${this.apiUrl}/${this.apiRoute}/${workspaceId}`, { headers })
      .pipe(
        tap((projects) => {
          this.projectsSubject.next(projects);
        }),
        map((response) => response || []),
        catchError(() => of([])),
      );
  }

  createProject(projectRequest: ProjectRequest): Observable<boolean> {
    const header = this.getAuthHeaders();
    return this.http
      .post(`${this.apiUrl}/${this.apiRoute}`, projectRequest, { headers: header })
      .pipe(
        tap(() => {
            this.projectsSubject.next(this.getProjects(projectRequest.workspaceId) as unknown as ProjectResponse[]);
        }),
        map(() => true),
        catchError(() => of(false)),
      );
  }
}
