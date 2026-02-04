import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AiAgentService } from '../../services/ai-agent.service';
import { NotificationService } from '../../services/notification';
@Component({
  selector: 'app-ai-agent',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ai-agent.component.html',
  styleUrls: ['./ai-agent.component.css']
})
export class AiAgentComponent {

  question = '';
  answer = '';
  loading = false;

  constructor(private service: AiAgentService, private notify: NotificationService) {}

  ask() {
    if (!this.question.trim()) {
      this.answer = 'कृपया प्रश्न टाइप करा किंवा बोला.';
      return;
    }

    this.loading = true;
    this.service.ask(this.question).subscribe({
      next: res => {
        this.answer = res.answer;
        this.loading = false;
      },
      error: () => {
         this.notify.showError('काहीतरी चूक झाली.');
        this.loading = false;
      }
    });
  }

  // 🎤 Speech to Text
 // 🎙️ Speech to Text (Input)
startVoice(langCode: string = 'mr-IN') {
  const SpeechRecognition = (window as any).webkitSpeechRecognition || (window as any).SpeechRecognition;
  
  if (!SpeechRecognition) {
    this.notify.showError('Mic not supported in this browser. Please use Chrome or Edge.');
    return;
  }

  const recognition = new SpeechRecognition();
  
  // Explicitly set the language (e.g., 'hi-IN', 'mr-IN', or 'en-US')
  recognition.lang = langCode;
  recognition.interimResults = false;

  recognition.start();

  recognition.onresult = (e: any) => {
    this.question = e.results[0][0].transcript;
    // Optional: Auto-process the command once it's captured
  };

  recognition.onerror = (err: any) => {
    console.error('Speech Recognition Error:', err.error);
    if(err.error === 'not-allowed') this.notify.showError('Permission to use microphone was denied.');
  };
}

// 🔊 Text to Speech (Output)
speak(langCode: string = 'en-US') {
  if (!this.answer) return;

  // Cancel any ongoing speech before starting new one
  speechSynthesis.cancel();

  const utter = new SpeechSynthesisUtterance(this.answer);
  utter.lang = langCode;

  // Optional: Select a specific voice for the language if available
  const voices = speechSynthesis.getVoices();
  const selectedVoice = voices.find(v => v.lang === langCode);
  if (selectedVoice) {
    utter.voice = selectedVoice;
  }

  speechSynthesis.speak(utter);
}
}
