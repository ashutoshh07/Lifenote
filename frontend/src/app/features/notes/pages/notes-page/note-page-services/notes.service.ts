import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, of, switchMap, tap } from 'rxjs';
import { environment } from '../../../../../../environments/environment';
import { INote, ICreateNoteDto, IUpdateNoteDto } from '../../../../../core/models/note.model';
import { AuthService } from '../../../../../core/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class NotesService {
  private http = inject(HttpClient);
  private _authService = inject(AuthService);
  private apiUrl = `${environment.apiHost}/Note`;
  
  // Signals for reactive state
  notes = signal<INote[]>([
    {
      content: 'jjj',
      id: 2,
      title: 'ggg',
      userId: 2
    }
  ]);
  searchTerm = signal<string>('');
  isLoading = signal<boolean>(false);

  getAllNotes(): Observable<INote[]> {
    this.isLoading.set(true);
    const userId = sessionStorage.getItem('userId');;
    return this.http.get<INote[]>(`${this.apiUrl}/${userId}`).pipe(
      tap(notes => {
        this.notes.set(notes);
        this.isLoading.set(false);
      })
    );
  }

  getNoteById(id: number): Observable<INote> {
    return this.http.get<INote>(`${this.apiUrl}/${id}`);
  }

  createNote(note: ICreateNoteDto): Observable<INote> {
    const id = this._authService.currentUserDetails().id;
    return this.http.post<INote>(`${this.apiUrl}/${id}`, note).pipe(
      tap(newNote => {
        this.notes.update(notes => [newNote, ...notes]);
      })
    );
  }

  updateNote(id: number, note: IUpdateNoteDto): Observable<INote> {
    const userId = sessionStorage.getItem('userId');
    return this.http.put<INote>(`${this.apiUrl}/${userId}/${id}`, note).pipe(
      switchMap((notes) => of(notes)),
      tap(updatedNote => {        
        this.notes.update(notes => 
          notes.map(n => n.id === id ? { ...updatedNote, content: updatedNote.content, id, title: updatedNote.title,
            isPinned: updatedNote.isPinned
           } : n)
        );
      })
    );
  }

  deleteNote(id: number): Observable<void> {
    const userId = sessionStorage.getItem('userId');;
    return this.http.delete<void>(`${this.apiUrl}/${userId}/${id}`).pipe(
      tap(() => {
        this.notes.update(notes => notes.filter(n => n.id !== id));
      })
    );
  }

  togglePin(id: number): Observable<INote> {
    const userId = sessionStorage.getItem('userId');;
    return this.http.patch<INote>(`${this.apiUrl}/${userId}/${id}/pin`, {}).pipe(
      tap(updatedNote => {
        this.notes.update(notes =>
          notes.map(n => n.id === id ? updatedNote : n)
        );
      })
    );
  }

  searchNotes(term: string): Observable<INote[]> {
    this.searchTerm.set(term);
    return this.http.get<INote[]>(`${this.apiUrl}/search?q=${term}`).pipe(
      tap(notes => this.notes.set(notes))
    );
  }
}
