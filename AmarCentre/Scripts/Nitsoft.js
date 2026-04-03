
        $(document).ready(function () {
            var dt = new Date();
            var time = dt.toDateString() + '  ' + dt.getHours() + ":" + dt.getMinutes() + ":" + Math.round(dt.getSeconds(), 2);
            $('.Time').text(time);
            //for clock start up
            setTimeout(function () { Hide_Alert() }, 7000);
            setTimeout(function () { time_display() }, 1000);
            //clock function
            function time_display() {
                var dt = new Date();
                var time = dt.toDateString() + '  ' + dt.getHours() + ":" + dt.getMinutes() + ":" + Math.round(dt.getSeconds(), 2);
                $('.Time').text(time);
                setTimeout(function () { time_display() }, 1000);
            }
            function Hide_Alert() {

                $('.alert').hide();
                setTimeout(function () { Hide_Alert() }, 7000);
            }
            $(".Main_menu").click(function () {
                if ($(this).children('.Sub_menu_Div').is(':visible')) {
                    $(this).children('.Sub_menu_Div').hide();
                }
                else {
                    $(".Sub_menu_Div").hide();
                    $(this).children('.Sub_menu_Div').show();
                }


            });
            $('.numbers_only').keydown(function (e) {
                console.log('fgdfg');
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $('.numbers_onlyPlusMinus').keydown(function (e) {
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190, 109, 173]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $('.read_Only').attr('readonly', true);
        });
function ToggleDiv() {
            $('.div_pop:hidden').show();
            setTimeout(function () { $(".div_pop").hide(); }, 2000);
}
function ToggleDivError() {
    $('.div_poperror:hidden').show();
    setTimeout(function () { $(".div_poperror").hide(); }, 2000);
}
function ToggleMasterDiv() {
    $('.divMasterPop:hidden').show();
    setTimeout(function () { $(".divMasterPop").hide(); }, 2000);
}
function pageLoad() {
            $('.div_items').click(function (e) {

                $('.div_items').css('background-color', 'White');
                $('.div_items').css('color', 'Black');
                $(this).css('background-color', '#0078d7');
                $(this).css('color', 'White');

            }
            );
    $('.numbers_only').keydown(function (e) {
                if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
    });

    $('.numbers_onlyPlusMinus').keydown(function (e) {
        if ($.inArray(e.keyCode, [8, 9, 27, 13, 110, 190, 109, 173]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $('.read_Only').attr('readonly', true);
}

        function ValidateCombo(sender, eventArgs) {
            var textInTheCombo = sender.get_text();
            if (textInTheCombo != '') {
                var item = sender.findItemByText(textInTheCombo);
                //if there is no item with that text
                sender.get_text()
                if (!item) {
                    sender.set_text("");
                    alert('Select from the list...');

                }
            }
        }

function OnClientKeyPressing(sender, args) {
            sender.showDropDown(); //show dropdown after entering some characters
        }
