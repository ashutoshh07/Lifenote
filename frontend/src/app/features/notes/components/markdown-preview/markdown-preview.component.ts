import { Component, effect, ElementRef, inject, input } from '@angular/core';
import { marked } from 'marked';
import hljs from 'highlight.js';

@Component({
  selector: 'app-markdown-preview',
  imports: [],
  templateUrl: './markdown-preview.component.html',
  styleUrl: './markdown-preview.component.scss',
})
export class MarkdownPreviewComponent {
  content = input.required<string>();
  private el = inject(ElementRef);

  constructor() {
    // Basic marked configuration
    marked.setOptions({
      breaks: true,
      gfm: true,
      pedantic: false
    });

    // Render and highlight
    effect(() => {
      const markdown = this.content();

      // Parse markdown to HTML
      const html = marked.parse(markdown) as string;

      // Set HTML
      const container = this.el.nativeElement.querySelector('.markdown-body');
      container.innerHTML = html;

      // Highlight all code blocks after rendering
      container.querySelectorAll('pre code').forEach((block: any) => {
        hljs.highlightElement(block);
      });
    });
  }
}
