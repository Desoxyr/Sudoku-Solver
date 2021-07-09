# Sudoku Solver

Sudoku Solver was a personal challenge in the first semester. The goal was to be able to automatically solve any sudoku by utilising basic solving techniques followed by a brute force algorithm.

<img src="https://i.imgur.com/UUxPkor.gif" alt="drawing" width="550"/>

## Constraints 
Because this was a personal project, there were no hard constraints. However, I did set myself some goals. 

- Basic Sudoku solving techniques must be used in order to quickly find the solutions. 
- You can enter a sudoku manually and lock the puzzle. This way you can attempt to solve the sudoku yourself.
- You can fetch the sudokus through an API.
- If a sudoku has a solution, the solver must be able to find it.
- The time it takes to solve a hard sudoku must be under a second.
- The UI is generated through code as opposed to a premade WinForms app.

The sudokus that are generated through the API sometimes have multiple solutions. In this case, the solver shows the first valid solution.

## Personal Goals
This project was my first experience working with classes. I tried to split up the board into tiles and sort these into rows, columns and blocks.  

Because I was very inexperienced at the time, the code is quite messy and does not comply with single responsibility principles. If I were to write it again I could definitely improve upon it.
