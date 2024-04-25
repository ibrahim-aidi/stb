import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, debounceTime, distinctUntilChanged, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  @Input() sidebar = false;
  searchText: string = "";
  suggestions: string[] = [];
  selectedIndex: number = -1; // Index of the selected suggestion
  @ViewChild('searchInput') searchInputRef!: ElementRef;

  constructor(private router: Router) {}

  onSearchChange(event: Event){

  }

  selectSuggestion(suggestion: string) {

  }

  navigateToSearch(searchText: string) {

  }


  getSuggestions(value: string){

  }

  onKeyDown(event: KeyboardEvent) {

  }

  onBlur() {

  }
}
