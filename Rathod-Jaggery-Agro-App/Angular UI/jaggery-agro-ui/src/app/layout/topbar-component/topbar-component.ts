import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { EventEmitter, Output } from '@angular/core';

@Component({
   selector: 'app-topbar',   // ✅ MUST MATCH HTML
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './topbar-component.html'
})
export class TopbarComponent {

  @Output() toggleSidebar = new EventEmitter();

  constructor(private translate: TranslateService) {}

  switchLang(lang: string) {
    this.translate.use(lang);
  }
}
