import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  template: `
    <footer class="footer">
      <div class="container">

        <!-- Brand -->
        <div class="brand animate-fade-up">
          <h5>Rathod Jaggery & Agro</h5>
          <p>Pure • Natural • Farmer-First Jaggery Products</p>
        </div>

        <!-- Social Icons -->
        <div class="social-links animate-fade-up delay-1">
          <a href="https://www.instagram.com/sachin.nay" target="_blank" aria-label="Instagram">
            <i class="fab fa-instagram"></i>
          </a>
          <a href="https://wa.me/919665520564" target="_blank" aria-label="WhatsApp">
            <i class="fab fa-whatsapp"></i>
          </a>
          <a href="https://www.linkedin.com/in/sachin-rathod-d19952191" target="_blank" aria-label="LinkedIn">
            <i class="fab fa-linkedin"></i>
          </a>
          <a href="https://facebook.com/yourpage" target="_blank" aria-label="Facebook">
            <i class="fab fa-facebook"></i>
          </a>
        </div>

        <!-- Divider -->
        <div class="divider"></div>

        <!-- Copyright -->
        <div class="copyright animate-fade-up delay-2">
          © 2025 Rathod Jaggery & Agro. All rights reserved.
        </div>

      </div>
    </footer>
  `,
  styles: [`
    .footer {
      background: linear-gradient(135deg, #198754, #157347);
      color: #ffffff;
      padding: 3rem 1rem 2rem;
      text-align: center;
    }

    .brand h5 {
      font-weight: 700;
      letter-spacing: 0.5px;
      margin-bottom: 0.3rem;
    }

    .brand p {
      font-size: 0.95rem;
      opacity: 0.9;
    }

    .social-links {
      margin: 1.5rem 0;
      display: flex;
      justify-content: center;
      gap: 1.2rem;
    }

    .social-links a {
      width: 42px;
      height: 42px;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      background: rgba(255,255,255,0.15);
      color: #fff;
      font-size: 1.2rem;
      transition: all 0.3s ease;
    }

    .social-links a:hover {
      background: #ffffff;
      color: #198754;
      transform: translateY(-4px) scale(1.05);
      box-shadow: 0 6px 16px rgba(0,0,0,0.25);
    }

    .divider {
      width: 80px;
      height: 3px;
      background: rgba(255,255,255,0.4);
      margin: 1.2rem auto;
      border-radius: 2px;
    }

    .copyright {
      font-size: 0.85rem;
      opacity: 0.85;
    }

    /* Animations */
    .animate-fade-up {
      animation: fadeUp 0.8s ease forwards;
      opacity: 0;
    }

    .delay-1 { animation-delay: 0.2s; }
    .delay-2 { animation-delay: 0.4s; }

    @keyframes fadeUp {
      from {
        transform: translateY(15px);
        opacity: 0;
      }
      to {
        transform: translateY(0);
        opacity: 1;
      }
    }

    /* Responsive tweaks */
    @media (max-width: 576px) {
      .brand h5 {
        font-size: 1.1rem;
      }
      .social-links a {
        width: 38px;
        height: 38px;
        font-size: 1rem;
      }
    }
  `]
})
export class FooterComponent {}
