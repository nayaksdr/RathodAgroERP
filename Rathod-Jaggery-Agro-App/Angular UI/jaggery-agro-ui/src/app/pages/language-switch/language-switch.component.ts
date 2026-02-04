import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { LanguageService } from '../../services/language';

@Component({
  selector: 'app-language-switch',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  template: `
    <div class="lang-box">
      <button mat-stroked-button (click)="change('en')">English</button>
      <button mat-stroked-button color="primary" (click)="change('mr')">मराठी</button>
      <button mat-stroked-button color="accent" (click)="change('hi')">हिंदी</button>
    </div>
  `,
  styles: [`
    .lang-box {
      display: flex;
      gap: 10px;
      justify-content: flex-end;
    }
  `]
})
export class LanguageSwitchComponent {

  constructor(private langService: LanguageService) {}

  change(lang: string) {
    this.langService.setLanguage(lang).subscribe(() => {
      // 🔄 Reload app so new culture is applied
      window.location.reload();
    });
  }
}
