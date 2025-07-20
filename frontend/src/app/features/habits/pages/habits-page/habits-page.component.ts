import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutService } from '../../../../core/services/layout.service';

@Component({
  selector: 'app-habits-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './habits-page.component.html',
  styleUrls: ['./habits-page.component.scss']
})
export class HabitsPageComponent implements OnInit {
  private layoutService = inject(LayoutService);

  ngOnInit(): void {
  }
}