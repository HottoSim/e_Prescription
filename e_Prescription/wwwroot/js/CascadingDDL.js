$(document).ready(function () {
    $('#province').attr('disabled', true);
    $('#city').attr('disabled', true);
    $('#suburb').attr('disabled', true);
    LoadProvince();
});

function LoadProvince() {
    $('#province').empty();

    $.ajax({
        url: '/Admission/GetProvinces',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#province').attr('disabled', false);
                $('#province').append('<option>--select province--</option>');
                $('#City').append('<option>--select city--</option>');
                $('#suburb').append('<option>--select suburb--</option>');
                $.each(response, function (i, data) {
                    $('#province').append('<option value=' + data.ProvinceId + '>' + data.ProvinceName + '</option> ');
                });
            }
            else {
                $('#province').attr('disabled', true);
                $('#city').attr('disabled', true);
                $('#suburb').attr('disabled', true);
                $('#province').append('<option>--provinces not available--</option>');
                $('#City').append('<option>--city not available--</option>');
                $('#suburb').append('<option>--suburb not available--</option>');
            }
        },
        error: function (error) {
            alert(error)
        }
    });
}

function LoadCities() {
    $('#city').empty();
    $('#suburb').empty();
    $('#suburb').attr('disabled', true);

    $.ajax({
        url: '/Admission/GetCities',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#province').attr('disabled', false);
                $('#province').append('<option>--select province--</option>');
                $('#City').append('<option>--select city--</option>');
                $('#suburb').append('<option>--select suburb--</option>');
                $.each(response, function (i, data) {
                    $('#province').append('<option value=' + data.ProvinceId + '>' + data.ProvinceName + '</option> ');
                });
            }
            else {
                $('#province').attr('disabled', true);
                $('#city').attr('disabled', true);
                $('#suburb').attr('disabled', true);
                $('#province').append('<option>--provinces not available--</option>');
                $('#City').append('<option>--city not available--</option>');
                $('#suburb').append('<option>--suburb not available--</option>');
            }
        },
        error: function (error) {
            alert(error)
        }
    });
}