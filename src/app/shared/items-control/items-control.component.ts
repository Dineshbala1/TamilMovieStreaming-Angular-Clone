import { Component, OnInit } from '@angular/core';
import { movieDetailsMock } from '../models/mock-movieDetails';

@Component({
  selector: 'app-items-control',
  templateUrl: './items-control.component.html',
  styleUrls: ['./items-control.component.css']
})
export class ItemsControlComponent implements OnInit {
  movieDetails;

  constructor() {
    this.movieDetails = movieDetailsMock;
   }

  ngOnInit() {
  }

}
