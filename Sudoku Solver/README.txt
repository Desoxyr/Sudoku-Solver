The sudoku solver was a first semester project. The goal was to automatically solve any sudoku using a custom algorithm.

The algorithm first tries to solve the sudoku using basic techniques, such as checking whether squares can only hold one possibility and if a number can only occur in one spot in a row, column or block. If it doesn't manage to solve it this way, it makes a copy of the current solution and brute forces a solution.

The sudokus provided by the API sometimes have multiple solutions. In this case the algorithm uses the first valid solution it finds.
