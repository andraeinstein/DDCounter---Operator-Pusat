Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmbayar
    Dim second As Integer = 1
    Dim detik As Integer = 1
    Dim cekpass As String
    Dim useraktif As String

    Sub lihatstatus()
        Call atur()
        SQLCari("select nama from harga.user where status=1")
        If objDataTable.Rows.Count > 0 Then
            useraktif = objDataTable.Rows(0).Item("nama").ToString
        End If
    End Sub
    Sub inputtransaksi()
        Dim i As Integer = DataGridView1.CurrentRow.Index
        SQLQuery("insert into counter_pulsa.transaksi(tgl_trx,jumlah,kode_trx,ket,nota) values(now(),0,2,''," & lblnota.Text & ")")
    End Sub

    Sub inputtransaksipulsa()
        Dim i As Integer = DataGridView1.CurrentRow.Index
        SQLQuery("insert into counter_pulsa.transaksi_pulsa(id_trx,kode_produk,no_tujuan,suplier) " _
            & "select t.id_trx,t.kode_produk,t.no_tujuan,s.id_suplier from harga.tampung t, " _
            & "counter_pulsa.suplier s where s.jenis=1")
    End Sub

    Sub inputoutbox()
        Dim i As Integer = DataGridView1.CurrentRow.Index
        SQLQuery("insert into counter_pulsa.sms_outbox(tgl_sms,tgl_input,no_hp,nama,pesan,com) " _
            & "select now(),now(),s.no_hp,s.nama,concat(t.kode_produk,'.',t.no_tujuan,';',s.pin),s.port " _
            & "from counter_pulsa.suplier s, harga.tampung t where s.jenis=1")
    End Sub

    Sub inputdepo()
        Call atur()
        Call lihatstatus()
        SQLQuery("insert into counter_pulsa.transaksi(tgl_trx,jumlah,kode_trx,ket,nota) values(now(),'" & txtdepo.Text & "',1,''," & lblnota.Text & ")")
        SQLQuery("insert into counter_pulsa.transaksi_deposit(id_trx,id_member,nama,suplier,operator) select last_insert_id(),'" & txtid.Text & "','" & txtnama.Text & "',2,'" & useraktif & "' from counter_pulsa.suplier where jenis=0")
        SQLQuery("insert into counter_pulsa.sms_outbox(tgl_sms,tgl_input,no_hp,nama,pesan,com) select now(),now(),no_hp,nama,concat('" & txtid.Text & "." & txtdepo.Text & "'),port from counter_pulsa.suplier where jenis=0")
        SkypeButton1.Enabled = False
    End Sub

    Sub hapustampung()
        Call atur()
        SQLQuery("truncate harga.tampung")
    End Sub

    Sub cekpassword()
        SQLCari("select id,kata_awal from harga.passotoritas where kata_awal='" & computeHash(Regex.Replace(txtpass.Text, "[^A-Za-z]", String.Empty)) & "' and id=1")
        If objDataTable.Rows.Count > 0 Then
            Try
                If Regex.Replace(txtpass.Text, "[^0-9]", String.Empty) = Date.Today.Day Then
                    cekpass = "benar"
                Else
                    cekpass = "salah"
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Sub gantitulisan()
        lblpesan.ForeColor = Color.Blue
        lblpesan.Text = "Transaksi telah di terima dan segera diproses !!"
        lblproses.Visible = False
        Timer3.Start()
        Timer2.Stop()
        lblpesan.Visible = True
        PictureBox1.BackgroundImage = DDCounter.My.Resources.Resources.notification_done
    End Sub

    Sub cek()
        Call atur()
        SQLCari("select ket from harga.akses")
        If objDataTable.Rows(0).Item("ket").ToString = "beres" Then
            SQLQuery("update harga.akses set ket='siap'")
            Call gantitulisan()
        ElseIf objDataTable.Rows(0).Item("ket").ToString = "benar" Then
            SQLQuery("update harga.akses set ket='siap'")
            Call gantitulisan()
            frmdepo.Close()
        ElseIf objDataTable.Rows(0).Item("ket").ToString = "batal" Then
            lblpesan.Text = "Transaksi telah dibatalkan !!"
            lblproses.Visible = False
            Timer3.Start()
            Timer2.Stop()
            lblpesan.Visible = True
            PictureBox1.BackgroundImage = DDCounter.My.Resources.Resources.notification_error
        End If
    End Sub

    Private Sub frmbayar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim total As Integer
        SkypeButton1.Enabled = True
        PictureBox1.BackgroundImage = DDCounter.My.Resources.Resources.notification_warning
        txtpass.Text = ""
        
        If transaksi = "pulsa" Then
            Groupdepo.Visible = False
            SQLCari("select t.id_trx as ID, o.nama as Operator, h.denom as Nominal, t.no_tujuan as Nomor" _
                                    & ", t.harga as Harga from harga.tampung t, harga.harga h, harga.operator o where t.kode_produk = h.kode_produk " _
                                    & "and h.id_operator=o.id_operator")
            DataGridView1.DataSource = objDataTable

            For Each dgvRow As DataGridViewRow In DataGridView1.Rows
                If Not dgvRow.IsNewRow Then
                    total += CInt(dgvRow.Cells(4).Value)
                End If
            Next
            lbltotal.Text = total
            lblpesan.ForeColor = Color.Red
            lblpesan.Text = "Silahkan bayar Rp " & lbltotal.Text & ",- ke kasir !"
            lblproses.Visible = True

            GroupBox1.Text = "Informasi Pembelian Pulsa"
            lblinformasi.Text = "Daftar pulsa yang dibeli"
            lblproses.Text = "Proses pengisian pulsa akan dilakukan setelah anda membayar sejumlah uang ........"
            txtterbilang.Text = Terbilang(total) + " Rupiah"
        Else
            'untuk deposit
            Groupdepo.Visible = True
            txtid.Text = frmdepo.txtid.Text
            txtnama.Text = frmdepo.txtnama.Text
            txtdepo.Text = frmdepo.txtjumlah.Text
            SQLCari("select idres 'ID Reseller',nama Nama,depo 'Jumlah Deposit' from harga.tampung2")
            DataGridView1.DataSource = objDataTable

            For Each dgvRow As DataGridViewRow In DataGridView1.Rows
                If Not dgvRow.IsNewRow Then
                    total += CInt(dgvRow.Cells(2).Value)
                End If
            Next
            lbltotal.Text = total
            lblpesan.ForeColor = Color.Red
            lblpesan.Text = "Silahkan bayar Rp " & lbltotal.Text & ",- ke kasir !"
            lblproses.Visible = True

            GroupBox1.Text = "Informasi Deposit"
            lblinformasi.Text = "Informasi uang deposit"
            lblproses.Text = "Proses penambahan deposit akan dilakukan setelah anda menyerahkan sejumlah uang."
            txtterbilang.Text = Terbilang(total) + " Rupiah"
        End If

        jumlahdata = DataGridView1.Rows.Count

        If transaksi = "pulsa" Then
            SQLCari("select (t.nota)+1 as nota from counter_pulsa.transaksi t where kode_trx=2 order by t.id_trx desc limit 1")
            If objDataTable.Rows.Count > 0 Then
                lblnota.Text = objDataTable.Rows(0).Item("nota").ToString
            Else
                lblnota.Text = 1
            End If
        Else
            SQLCari("select (t.nota)+1 as nota from counter_pulsa.transaksi t where kode_trx=1 order by t.id_trx desc limit 1")
            If objDataTable.Rows.Count > 0 Then
                lblnota.Text = objDataTable.Rows(0).Item("nota").ToString
            Else
                lblnota.Text = 1
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'second = second - 1
        'If second = 0 Then
        '    cek()
        '    second = 1
        'End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        If lblpesan.Visible = True Then
            lblpesan.Visible = False
        ElseIf lblpesan.Visible = False Then
            lblpesan.Visible = True
        End If
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        detik = detik + 1
        If detik = 8 Then
            detik = 1
            Timer3.Stop()
            Me.Close()
            Timer2.Start()
        End If
    End Sub

    Private Sub SkypeButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton1.Click
        'Call atur()
        SplashScreenManager1.ShowWaitForm()
        Call cekpassword()
        If txtpass.Text = "" Then
            MessageBox.Show("Password tidak boleh kosong !!", "Set Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If cekpass = "benar" Then
                'Try
                SQLQuery("update harga.akses set ket='beres'")
                If objDataTable.Rows.Count > 0 Then
                    Dim i As Integer = DataGridView1.CurrentRow.Index

                    If transaksi = "pulsa" Then
                        For a As Integer = 1 To jumlahdata
                            Call inputtransaksi()
                        Next
                        Call inputtransaksipulsa()
                        Call inputoutbox()
                        Call hapustampung()
                    Else
                        Call inputdepo()
                    End If
                End If
                Call gantitulisan()
                'Me.Close()
                jumlahdata = DataGridView1.RowCount
                SplashScreenManager1.CloseWaitForm()
                If transaksi = "pulsa" Then
                    struk.ShowDialog()
                Else
                    strukdepo.ShowDialog()
                End If
            Else
                SplashScreenManager1.CloseWaitForm()
                MessageBox.Show("Password salah !!", "Set Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

    End Sub

    Private Sub SkypeButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkypeButton2.Click
        Call atur()
        If transaksi = "pulsa" Then
            SQLQuery("update harga.akses set ket='batal'")
            MessageBox.Show("Pembelian pulsa telah dibatalkan !", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Call hapustampung()
        Else
            MessageBox.Show("Penambahan saldo deposit telah dibatalkan !", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If
    End Sub
End Class