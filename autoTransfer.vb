'@author: Adonis Settouf
'@mail: adonis.settouf@gmail.com

Sub writeDatas()
    Dim val As Variant, KAVal As Variant, row As Long, BAUCallOffered As String, KARange As String, BAUVal As Variant, KACountry As Boolean
    Dim wb As Workbook
    Dim KAList As Variant
    Application.Calculation = xlCalculationManual
    KAList = Array("France", "UK", "Germany", "Spain", "Portugal", "Belgium", "Netherlands", "Norway")
    Set wb = findWorkbook("phone_report")
    val = wb.Worksheets("PSSD combined").Range("B4:G22").value
    
    
    BAUCallOffered = "BAU Call offered"
    KARange = "KA Call offered"
    KAVal = wb.Worksheets("PSSD KA").Range("B4:G22").value
    BAUVal = wb.Worksheets("PSSD BAU").Range("B4:G22").value
    For i = 1 To (UBound(KAVal, 1))
         For Each country In KAList
            If (InStr(KAVal(i, 1), country)) > 0 Then
                Call writeToSheet(BAUCallOffered, BAUVal(i, UBound(BAUVal, 2)), KAVal(i, 1))
                Call writeToSheet(KARange, KAVal(i, UBound(KAVal, 2)), KAVal(i, 1))
                KACountry = True
                Exit For
            End If
        KACountry = False
        Next country
        If Not KACountry Then
            Call writeToSheet(BAUCallOffered, val(i, UBound(val, 2)), val(i, 1))
        End If
    Next
    Application.Calculation = xlCalculationAutomatic
End Sub

'writeToTheGlobalForecast Workbook
Function writeToSheet(ByVal colName As String, ByVal value As Integer, ByVal country As String)
    Dim ran As String, row As Long, realCountry As String
    row = findRangeToWrite()
    If (InStr(country, "Ireland") > 0) Then
       realCountry = "UK"
    ElseIf (InStr(country, "Africa") > 0) Then
       realCountry = "South Africa"
    Else
       realCountry = country
    End If
    If (InStr(country, "LexLIME") > 0) Then
    Else
        
        ran = findColumnLetter(colName, ThisWorkbook.Worksheets(realCountry)) & CStr(row)
        ThisWorkbook.Worksheets(realCountry).Range(ran).value = value
    End If
    
    
End Function

'Find the good row to write datas
Function findRangeToWrite() As Long
    Dim lastRow As Long, firstRow As Long, counter As Long
    Dim val As Variant
    counter = 0
    lastRow = ThisWorkbook.Worksheets("France").Cells.Find("*", searchorder:=xlByRows, searchdirection:=xlPrevious).row
    firstRow = lastRow - 23
    val = ThisWorkbook.Worksheets("France").Range("A" & CStr(firstRow) & ":B" & CStr(lastRow)).value
    'MsgBox (InStr(val(1, 1), CStr(Year(Date))))
    For i = 1 To UBound(val, 1)
        If (InStr(val(i, 1), CStr(Year(Date))) > 0 And InStr(MonthName(Month(Date) - 1), val(i, 2)) > 0) Then
            counter = i
            Exit For
        End If
    Next
    findRangeToWrite = firstRow + counter - 1
End Function

'find Workbooks with names (partial name ok)
Function findWorkbook(ByVal nameWB As String) As Workbook
    Dim buffWb As Workbook
    For Each book In Workbooks
        If (InStr(book.name, nameWB) > 0) Then
            Set findWorkbook = book
            Exit For
        End If
    Next book
End Function

'Function to find column letter with a column name in the two first rows in a given worksheet (watch out, check only on the two first rows
'and return first one that contains the searched string, so be very specific
Function findColumnLetter(ByVal name As String, ByVal ws As Worksheet) As String
    Dim counter As Integer
    counter = 1
    For i = 1 To 2
        For j = 1 To ws.Columns.Count
            If InStr(ws.Cells(i, j).value, name) > 0 Then
                counter = j
                Exit For
            End If
        Next
    Next
    findColumnLetter = Chr(64 + counter)
End Function

'Proc for testing new func
Sub test()
   Dim col As String
   col = findColumnLetter("BAU NVC", ThisWorkbook.Worksheets("France"))
   MsgBox (col)
End Sub

Private Sub TransferScript_Click()
    writeDatas
End Sub




