
import {
  Component,
  Input,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  ViewChild,
  ElementRef
} from '@angular/core';   
import { CommonModule } from '@angular/common';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-produce-chart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './produce-chart.html',
  styleUrl: './produce-chart.css',
})
export class ProduceChartComponent implements AfterViewInit, OnChanges {

  @Input() data: any[] = [];

  @ViewChild('chartCanvas') canvas!: ElementRef<HTMLCanvasElement>;

  private chart!: Chart;

  ngAfterViewInit() {
    if (this.data?.length) {
      this.renderChart();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data'] && this.canvas) {
      this.chart?.destroy();
      this.renderChart();
    }
  }

  private renderChart() {
    this.chart = new Chart(this.canvas.nativeElement, {
      type: 'bar',
      data: {
        labels: this.data.map(x =>
          new Date(x.producedDate).toLocaleDateString()
        ),
        datasets: [{
          label: 'Quantity (Kg)',
          data: this.data.map(x => x.quantityKg),
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false
      }
    });
  }
}
