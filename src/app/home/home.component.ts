import { Component, OnInit} from '@angular/core';
import { movieDetailsMock } from '../shared/models/mock-movieDetails';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  movieDetails;

  constructor() {
    this.movieDetails = movieDetailsMock;
  }

  ngOnInit() {

  }

}
