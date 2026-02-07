import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { INote, ICreateNoteDto } from '../../../../core/models/note.model';
import { NoteCardComponent } from '../../components/note-card/note-card.component';
import { MarkdownPreviewComponent } from '../../components/markdown-preview/markdown-preview.component';
import { LucideAngularModule, Search, Plus, X, Eye, Edit3 } from 'lucide-angular';
import { NotesService } from './note-page-services/notes.service';

@Component({
  selector: 'app-notes-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NoteCardComponent,
    MarkdownPreviewComponent,
    LucideAngularModule
  ],
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.scss']
})
export class NotesPageComponent implements OnInit {
  private notesService = inject(NotesService);

  // Icons
  SearchIcon = Search;
  PlusIcon = Plus;
  CloseIcon = X;
  EyeIcon = Eye;
  EditIcon = Edit3;

  // Signals
  searchQuery = signal('');
  showEditor = signal(false);
  editingNote = signal<INote | null>(null);
  isPreviewMode = signal(false);

  // Editor form
  editorTitle = signal('');
  editorContent = signal('');

  // Computed
  notes = computed(() => this.notesService.notes());
  filteredNotes = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) return this.notes();

    return this.notes().filter(note =>
      note.title.toLowerCase().includes(query) ||
      note.content.toLowerCase().includes(query) ||
      note.tags?.some(tag => tag.toLowerCase().includes(query))
    );
  });

  pinnedNotes = computed(() =>
    this.filteredNotes().filter(n => n.isPinned)
  );

  regularNotes = computed(() =>
    this.filteredNotes().filter(n => !n.isPinned)
  );

  ngOnInit() {
    this.loadNotes();
  }

  loadNotes() {
    this.notesService.getAllNotes().subscribe();
  }

  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  openNewNote() {
    this.editingNote.set(null);
    this.editorTitle.set('');
    this.editorContent.set('');
    this.isPreviewMode.set(false);
    this.showEditor.set(true);
  }

  openEditNote(note: INote) {
    this.editingNote.set(note);
    this.editorTitle.set(note.title);
    this.editorContent.set(note.content);
    this.isPreviewMode.set(false);
    this.showEditor.set(true);
  }

  closeEditor() {
    this.showEditor.set(false);
    this.editingNote.set(null);
  }

  togglePreview() {
    this.isPreviewMode.update(v => !v);
  }

  saveNote() {
    const noteData = {
      title: this.editorTitle(),
      content: this.editorContent()
    };

    if (this.editingNote()) {
      // Update existing
      this.notesService.updateNote(this.editingNote()!.id, noteData).subscribe({
        next: () => this.closeEditor(),
        error: (err) => console.error('Failed to update note', err)
      });
    } else {
      // Create new
      this.notesService.createNote(noteData as ICreateNoteDto).subscribe({
        next: () => this.closeEditor(),
        error: (err) => console.error('Failed to create note', err)
      });
    }
  }

  deleteNote(id: number) {
    if (confirm('Delete this note?')) {
      this.notesService.deleteNote(id).subscribe({
        error: (err) => console.error('Failed to delete note', err)
      });
    }
  }

  togglePin(id: number) {
    this.notesService.togglePin(id).subscribe({
      error: (err) => console.error('Failed to toggle pin', err)
    });
  }
}
