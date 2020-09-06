Option Explicit
'ensures board is valid
Public Function ensureValid()
    
    Dim SKIP As Boolean
    
    Dim row As Integer 'current row position
    Dim col As Integer 'current column position
    Dim current As Integer 'try value (1-9)
    Dim xCheck As Integer 'check row
    Dim yCheck As Integer 'check column
    Dim sqCheck As Integer 'check 3x3 block
    Dim D As Integer
    Dim x As Integer
    Dim y As Integer
    Dim k As Integer
    Dim j As Integer
    
    SKIP = False
    
    row = 2 'starts row 1
    Do While (row <= 10) 'move one over one column
        
        col = 1 'reset column
        Do While (col <= 9) 'move down one row
        
            current = CInt(Cells(row, col)) 'look at current value
            
            If (current = 0) Then 'blank cell - SKIP
            
                col = col + 1
                
            Else 'check if valid on board
                
                
                ''''''''''''''''''''''''''''''''''''''
                ''''''''''check valid in row''''''''''
                ''''''''''''''''''''''''''''''''''''''
                
                If (SKIP <> True) Then
                
                    D = 1
                    Do While (D <= 9)
                    
                        If (D = col) Then 'current cell - SKIP
                        
                            D = D + 1
                            
                        Else
                    
                            xCheck = CInt(Cells(row, D))
                            
                            If (xCheck = current) Then 'found in row - INVALID
                            
                                FAIL = True
                                SKIP = True
                                D = 10
                                
                            Else
                            
                                D = D + 1
                            
                            End If
                            
                        End If
                        
                    Loop
                    
                End If
                
                ''''''''''''''''''''''''''''''''''''''
                ''''''''check valid in column'''''''''
                ''''''''''''''''''''''''''''''''''''''
                
                If (SKIP <> True) Then
                
                    D = 2 'board starts on second row down
                    Do While (D <= 10)
                    
                        If (D = row) Then 'current cell - SKIP
                        
                            D = D + 1
                            
                        Else
                    
                            yCheck = CInt(Cells(D, col))
                            
                            If (yCheck = current) Then 'found in column - INVALID
                            
                                FAIL = True
                                SKIP = True
                                D = 11
                                
                            Else
                            
                                D = D + 1
                            
                            End If
                            
                        End If
                        
                    Loop
                    
                End If
                
                ''''''''''''''''''''''''''''''''''''''
                '''''''''check valid in square''''''''
                ''''''''''''''''''''''''''''''''''''''
                
                If (SKIP <> True) Then
                
                    y = 3 * Int((row - 2) / 3) + 2
                    x = 3 * Int((col - 1) / 3) + 1
                    
                    k = 0
                    Do While (k <= 2) 'move down one row in 3x3 block
                    
                        j = 0
                        Do While (j <= 2) 'move right one column in 3x3 block
                            
                            If (x = col) And (y = row) Then 'current cell - SKIP
                                
                                j = j + 1
                                x = x + 1
                                
                            Else
                            
                                sqCheck = CInt(Cells(y, x))
                                
                                If (sqCheck = current) Then 'found in 3x3 block - INVALID
                                
                                    FAIL = True
                                    SKIP = True
                                    j = 4
                                    k = 4
                                    
                                Else
                                
                                    j = j + 1
                                    x = x + 1
                                    
                                End If
 
                            End If
                            
                            If (j = 3) Then 'reset x
                                    
                                x = 3 * Int((col - 1) / 3) + 1
                                
                            End If
                            
                        Loop
                        
                        k = k + 1
                        y = y + 1
                        
                    Loop
                    
                    '''''''''''''''past cell checks''''''''''''''''''''''
                
                End If
                
                col = col + 1
                
            End If
            
        Loop
        
        row = row + 1
        
    Loop
    
    ''''''''''''''''''''''''''''''''''''''
    '''''''''valid if FAIL <> True''''''''
    ''''''''''''''''''''''''''''''''''''''
    
    If (FAIL = True) Then

        'display failed message
        genMsg = MsgBox("Board is invalid! Check board and try again!", vbOKOnly, "INVALID BOARD")
        
        Exit Function
        
    Else
        
        If (valMsg = True) Then 'display valid message
        
            'display success message
            genMsg = MsgBox("VALID BOARD!!", vbOKOnly, "VALID")
            
        End If
        
    End If
    
End Function

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

'solves sudoku puzzle
Public Function solvePuzzle()
    
    If (FAIL = True) Then 'puzzle not possible dont attempt
        Exit Function
    End If
    
    Dim SKIP As Boolean 'used to skip future checks on a number
    Dim BACKTRACK As Boolean 'flag that code is running backwards through puzzle
    Dim backTracking As Boolean 'flad that code needs to continue backtracking
    Dim GOLD As Boolean 'used to determine previously placed number
    
    Dim row As Integer 'current row position
    Dim col As Integer 'current column position
    Dim try As Integer 'value to try in cell (1-9)
    Dim current As Integer 'current position cell contents
    Dim xCheck As Integer 'check row
    Dim yCheck As Integer 'check column
    Dim sqCheck As Integer 'check 3x3 block
    Dim D As Integer 'used to check rows/columns
    Dim x As Integer 'used to check 3x3 square
    Dim y As Integer 'used to check 3x3 square
    Dim k As Integer 'used for loop check 3x3 square
    Dim j As Integer 'used for loop check 3x3 square
    Dim v As Integer 'used for finding valid cell for backtracking
    Dim fontSize As Integer 'used to tell if code is backtracking
    
    SKIP = False
    BACKTRACK = False
    backTracking = False
    
    row = 2
    col = 1
    
    Do While (row <= 10) 'move down 1 row
        
        col = 1
        Do While (col <= 9) 'move over 1 column
        
            current = CInt(Cells(row, col)) 'get value of current cell
            fontSize = CInt(Cells(row, col).Font.Size) 'gets the font size
            
            If (fontSize = 39) Then 'valid cell for BACKTRACK code
            
                GOLD = True
                
            End If
            
            If (current <> 0) And (BACKTRACK <> True) Then 'not blank space or backtracking - SKIP
            
                col = col + 1
                
            Else
                
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''ENSURE BACKTRACKED CELL IS VALID SPACE''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                
                If (BACKTRACK = True) Then
                    
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '''''''''MOVE BACK ONE CELL IF NOT A NUMBER WE PLACED'''''''''
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    
                    If (GOLD <> True) Then 'keep moving back through puzzle until spot is found
                    
                        Do While (v <= 81) 'how many placements we can go back
                        
                            ''''STEP BACK THROUGH CODE UNTIL CELL IS FOUND THAT IS GOLD = True''''
                            
                            If (col = 1) Then
                                
                                'up one row far right cell
                                col = 9
                                row = row - 1
                                
                                If (row = 1) Then 'no solution
                                
                                    genMsg = MsgBox("Well this means the code backtracked past the beginning of the puzzle. No solution to this possible was found. Sorry :(", vbOKOnly, "WELL THIS IS EMBARRASING")
                                    
                                    Exit Function
                                    
                                Else
                                    
                                    'check if valid space
                                    fontSize = CInt(Cells(col, row).Font.Size)
                                    
                                    If (fontSize = 39) Then 'gold space - VALID
                                    
                                        GOLD = True
                                        
                                    End If
                                    
                                End If
                                
                            Else
                                
                                'move left one column
                                col = col - 1
                                
                                'check if valid
                                fontSize = CInt(Cells(col, row).Font.Size)
                                
                                If (fontSize = 39) Then 'gold space - VALID
                                
                                    GOLD = True
                                    
                                End If
                                
                            End If
                                    
                            If (GOLD = True) Then 'found valid space
                            
                                Exit Do
                                try = CInt(Cells(row, col)) + 1
                                
                            Else 'try next space back
                                
                                v = v + 1
                                
                            End If
                            
                        Loop
                        
                    Else 'CURRENT CELL VALID FOR BACKTRACKING - TRY NEXT NUMBER
                
                        try = CInt(Cells(row, col)) + 1
                        'reset backTracking
                        
                    End If
                        
                    If (try = 10) Then 'cant try next number, need to backtrack again
                    
                        backTracking = True
                        BACKTRACK = True
                        
                    Else
                    
                        backTracking = False
                        BACKTRACK = False
                        
                    End If
                    
                Else 'not backtracking - start at 1
                
                    try = 1
                    
                End If
                
                If (backTracking = False) Then 'check try value
                
                    SKIP = False
                    Do While (try <= 9) 'try numbers 1-9
                        
                            ''''''''''''''''''''''''''''''''''''''
                            ''''check if 'try' is valid in row''''
                            ''''''''''''''''''''''''''''''''''''''
                            
                        'SKIP = False 'reset skip (only before row!)
                        D = 1 'reset D
                        Do While (D <= 9) 'move over one column
                        
                            xCheck = CInt(Cells(row, D))
                            
                            If (xCheck = try) Then 'try found in row - INVALID
                            
                                SKIP = True
                                Exit Do
                                
                            Else
                            
                                D = D + 1
                            
                            End If
                            
                        Loop
                        
                        '''''''''''''''''''''''''''''''''''''''
                        '''check if 'try' is valid in column'''
                        '''''''''''''''''''''''''''''''''''''''
                        
                       If (SKIP <> True) Then 'try not found in row and not backtracking
                        
                            D = 2 'reset D
                            Do While (D <= 10) 'move down one row
                            
                                yCheck = CInt(Cells(D, col))
                                
                                If (yCheck = try) Then 'found in column - INVALID
                                
                                    SKIP = True
                                    Exit Do
                                    
                                Else
                                
                                    D = D + 1
                                    
                                End If
                                
                            Loop
                            
                        End If
                        
                        ''''''''''''''''''''''''''''''''''''''''''
                        '''check if 'try' is valid in 3x3 block'''
                        ''''''''''''''''''''''''''''''''''''''''''
                        
                        If (SKIP <> True) Then 'try not found in row or column not backtracking
                            
                            'feed in coordinates (row, col) and get top left
                            'cell coordinates of the current 3x3 block (x, y)
                            x = 3 * Int((col - 1) / 3) + 1
                            y = 3 * Int((row - 2) / 3) + 2
                            
                            k = 1
                            Do While (k <= 3) 'move down one row in 3x3
                            
                                j = 1 'reset column
                                Do While (j <= 3) 'move over one column
                                
                                    sqCheck = CInt(Cells(y, x))
                                    
                                    If (sqCheck = try) Then 'found in square - INVALID
                                    
                                        SKIP = True
                                        j = 4
                                        k = 4
                                        
                                    Else
                                    
                                        j = j + 1
                                        x = x + 1
                                        
                                        If (j = 4) Then 'reset x
                                        
                                            x = 3 * Int((col - 1) / 3) + 1
                                            
                                        End If
                                        
                                    End If
                                    
                                Loop
                                
                                k = k + 1
                                y = y + 1
                                
                            Loop
                            
                        End If
                        
                        ''''''''''''''''''''''''''''''''''''''''''
                        ''past checks - see if 'try' was skipped''
                        ''''''''''''''''''''''''''''''''''''''''''
                        
                        If (SKIP <> True) Then 'try made it through checks is VALID
                        
                            'fill in cell with try
                            With Cells(row, col)
                                .Value = try
                                .Font.Color = RGB(208, 154, 0)
                                .Font.Size = 39
                            End With
                            
                            'reset backtrack
                            BACKTRACK = False
                            
                            Exit Do 'dont try any more numbers
                            
                        Else 'try was skipped, move to next number

                            try = try + 1
                            SKIP = False 'reset skip
                            
                            If (try = 10) Then 'HERE IS WHERE I WILL NEED TO IMPLEMENT BACKTRACKING
                            'IF try = 10 THEN ALL NUMBERS HAVE BEEN TRIED AND WE NEED TO GO BACK TO
                            'OUR LAST CHOICE TO TRY A DIFFERENT CHOICE. WHEN A CHOICE IS MADE THE FONT
                            'SIZE IS CHANGED. I'VE USED THIS AS A FLAG TO TELL ME IF I CAN CHANGE A
                            'NUMBER IN THE CELL BASED ON FONT SIZE
                            
                                BACKTRACK = True
                                
                                'reset current cell to 0
                                With Cells(row, col)
                                    .Value = 0
                                    .Font.Size = 37
                                    .Font.Color = RGB(52, 131, 202)
                                End With
                                
                            Else
                            
                                BACKTRACK = False
                                    
                            End If

                        End If
                        
                    Loop
                    
                End If
                    
                If (BACKTRACK = False) Then 'going forward in puzzle
                    
                    col = col + 1
                    
                Else 'going backwards in puzzle
                
                    If (col = 1) Then
                    
                        'move up one row far right cell
                        col = 9
                        row = row - 1
                        
                        If (row = 1) Then 'no solution
                        
                           genMsg = MsgBox("Well this means the code backtracked past the beginning of the puzzle. No solution to this possible was found. Sorry :(", vbOKOnly, "WELL THIS IS EMBARRASING")
                           
                           Exit Function
                           
                        End If
                        
                    Else
                        
                        'move back one column
                        col = col - 1
                        
                    End If
                    
                End If
                
            End If
            
        Loop
        
        If (BACKTRACK = False) Then 'going forward in puzzle
            
            row = row + 1
        
        End If
        
    Loop

End Function