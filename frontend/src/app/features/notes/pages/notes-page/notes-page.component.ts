import { Component, inject, signal, computed, OnInit, effect, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { INote, ICreateNoteDto } from '../../../../core/models/note.model';
import { NoteCardComponent } from '../../components/note-card/note-card.component';
import { MarkdownPreviewComponent } from '../../components/markdown-preview/markdown-preview.component';
import { LucideAngularModule, Search, Plus, X, Eye, Edit3, Maximize2 } from 'lucide-angular';
import { NotesService } from './note-page-services/notes.service';
import { BreakpointService } from '../../../../core/services/breakpoint.service';
import { Subject, debounceTime } from 'rxjs';

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
  private breakpointService = inject(BreakpointService);
  private autoSaveSubject = new Subject<void>();

  // Icons
  PlusIcon = Plus;
  CloseIcon = X;
  EyeIcon = Eye;
  EditIcon = Edit3;
  FullscreenIcon = Maximize2;

  // Signals
  searchQuery = signal('');
  showEditor = signal(false);
  editingNote = signal<INote | null>(null);
  isPreviewMode = signal(false);
  isFullscreen = signal(false);

  // Inline note creator state
  isInlineCreatorExpanded = signal(false);
  inlineTitle = signal('');
  inlineContent = signal('');

  // Editor form
  editorTitle = signal('');
  editorContent = signal('');

  // Computed
  notes = computed(() => this.notesService.notes());
  isMobile = computed(() => this.breakpointService.isMobile());
  hasNotes = computed(() => this.notes().length > 0);
  showFAB = computed(() => this.isMobile() || !this.hasNotes());

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

  constructor() {
    // Auto-save for modal editor
    this.autoSaveSubject.pipe(debounceTime(1000)).subscribe(() => {
      this.autoSaveModalNote();
    });

    // Watch for modal editor changes
    effect(() => {
      if (this.showEditor() && this.editingNote()) {
        this.editorTitle();
        this.editorContent();
        this.autoSaveSubject.next();
      }
    });
  }

  ngOnInit() {
    this.loadNotes();

    document.addEventListener('click', (e) => this.handleGlobalClick(e));

    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape') {
        if (this.isFullscreen()) {
          this.isFullscreen.set(false);
        } else if (this.showEditor()) {
          this.closeEditor();
        } else if (this.isInlineCreatorExpanded()) {
          this.closeInlineCreator();
        }
      }
    });
  }

  // Handle clicks outside inline creator
  private handleGlobalClick(event: MouseEvent) {
    if (!this.isInlineCreatorExpanded()) return;

    const target = event.target as HTMLElement;
    const inlineCreator = document.querySelector('.inline-creator-expanded');

    // Check if click is outside the inline creator
    if (inlineCreator && !inlineCreator.contains(target)) {
      this.closeInlineCreator();
    }
  }

  loadNotes() {
    this.notesService.getAllNotes().subscribe();
  }

  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  // Inline creator methods
  expandInlineCreator() {
    this.isInlineCreatorExpanded.set(true);
  }

  closeInlineCreator() {
    this.saveInlineNote();
  }

  saveInlineNote() {
    const title = this.inlineTitle();
    const content = this.inlineContent();

    // Don't save if both are empty
    if (!title.trim() && !content.trim()) {
      this.isInlineCreatorExpanded.set(false);
      this.inlineTitle.set('');
      this.inlineContent.set('');
      return;
    }

    const noteData: ICreateNoteDto = {
      title: title || 'Untitled',
      content: content
    };

    this.notesService.createNote(noteData).subscribe({
      next: () => {
        this.isInlineCreatorExpanded.set(false);
        this.inlineTitle.set('');
        this.inlineContent.set('');
      },
      error: (err) => console.error('Failed to create note', err)
    });
  }

  openNewNote() {
    this.editingNote.set(null);
    this.editorTitle.set('');
    this.editorContent.set('');
    this.isPreviewMode.set(false);
    this.isFullscreen.set(false);
    this.showEditor.set(true);
  }

  openEditNote(note: INote) {
    this.editingNote.set(note);
    this.editorTitle.set(note.title);
    this.editorContent.set(note.content);
    this.isPreviewMode.set(false);
    this.isFullscreen.set(false);
    this.showEditor.set(true);
  }

  closeEditor() {
    if (this.editingNote()) {
      this.autoSaveModalNote();
    } else {
      // For new notes, save if there's content
      const title = this.editorTitle();
      const content = this.editorContent();

      if (title.trim() || content.trim()) {
        const noteData: ICreateNoteDto = {
          title: title || 'Untitled',
          content: content
        };
        this.notesService.createNote(noteData).subscribe({
          error: (err) => console.error('Failed to create note', err)
        });
      }
    }

    this.showEditor.set(false);
    this.editingNote.set(null);
    this.isFullscreen.set(false);
  }

  onBackdropClick(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeEditor();
    }
  }

  togglePreview() {
    this.isPreviewMode.update(v => !v);
  }

  toggleFullscreen() {
    this.isFullscreen.update(v => !v);
  }

  private autoSaveModalNote() {
    if (!this.editingNote()) return;

    const title = this.editorTitle();
    const content = this.editorContent();

    if (!title.trim() && !content.trim()) return;

    const noteData = {
      title: title || 'Untitled',
      content: content,
      isPinned: this.editingNote()!.id ? this.editingNote()?.isPinned : false 
    };

    this.notesService.updateNote(this.editingNote()!.id, noteData).subscribe({
      error: (err) => console.error('Failed to auto-save note', err)
    });
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
