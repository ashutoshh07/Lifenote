import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, of, switchMap, tap } from 'rxjs';
import { environment } from '../../../../../../environments/environment';
import { INote, ICreateNoteDto, IUpdateNoteDto } from '../../../../../core/models/note.model';

@Injectable({
  providedIn: 'root',
})
export class NotesService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiHost}/Note`;
  
  // Signals for reactive state (user id is derived server-side from auth token)
  notes = signal<INote[]>([]);
  searchTerm = signal<string>('');
  isLoading = signal<boolean>(false);

  getAllNotes(): Observable<INote[]> {
    this.isLoading.set(true);
    return this.http.get<INote[]>(this.apiUrl).pipe(
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
    return this.http.post<INote>(this.apiUrl, note).pipe(
      tap(newNote => {
        this.notes.update(notes => [newNote, ...notes]);
      })
    );
  }

  updateNote(id: number, note: IUpdateNoteDto): Observable<INote> {
    return this.http.put<INote>(`${this.apiUrl}/${id}`, note).pipe(
      switchMap((updatedNote) => of(updatedNote)),
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
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => {
        this.notes.update(notes => notes.filter(n => n.id !== id));
      })
    );
  }

  togglePin(id: number): Observable<INote> {
    return this.http.patch<INote>(`${this.apiUrl}/${id}/pin`, {}).pipe(
      tap(updatedNote => {
        this.notes.update(notes =>
          notes.map(n => n.id === id ? updatedNote : n)
        );
      })
    );
  }

  searchNotes(term: string): Observable<INote[]> {
    this.searchTerm.set(term);
    return this.http.get<INote[]>(`${this.apiUrl}/search?q=${encodeURIComponent(term)}`).pipe(
      tap(notes => this.notes.set(notes))
    );
  }
}
