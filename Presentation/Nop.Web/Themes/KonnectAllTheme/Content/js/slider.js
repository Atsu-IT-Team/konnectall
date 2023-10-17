
var Slideindex = 0;
var sliders = document.getElementsByClassName("sec-desc-categ");
var hideSlide = () => {
    var sliders = document.getElementsByClassName("sec-desc-categ");
    for (let index = 1; index < sliders.length; index++) {
       var slid = sliders[index];
        slid.style.display ="none";
    }
}

var dotdisplay = () => {
    var sliders = document.getElementsByClassName("sec-desc-categ");
    for (let index = 0; index < sliders.length; index++) {
        var slid = sliders[index];  
        var li = document.createElement('li');
        li.setAttribute('class', 'dot');
        document.getElementById('dots-item')?.appendChild(li);
        if(index === 0) {
            li.classList.add("active");
            slid.style.display = "flex";
        }
        li.onclick = () => {
            shownSlide(index);
        }
    }
}

var shownSlide = (index) => {
    var sliders = document.getElementsByClassName("sec-desc-categ");  
    if (typeof(index) !== "number"){
        return
    }
    index = index%sliders.length;
    var lastIndex = Slideindex;
    Slideindex = index;
    sliders[lastIndex].style.display = "none";
    sliders[Slideindex].style.display = "flex";
    setActiveSlide(lastIndex, Slideindex);
}

var changeSlide = () => {
    var index = Slideindex
    shownSlide(index)
}

//var count = 1;

function plus() {
    var countEl = document.getElementById("count");
    var count = countEl.value;
    count++;
    countEl.value = count;
}
function minus() {
    var countEl = document.getElementById("count");
    var count = countEl.value;
    if (count > 1) {
        count--;
        countEl.value = count;
    }
}

function showAccordContent(ev) {
    var acc = document.getElementsByClassName("accordion");
    var panel = document.getElementsByClassName('panel');
    var accordeon_titles = document.querySelectorAll('.accord-title');
    var accordeon_titles_child = document.querySelectorAll('.accord-title-child');
    var accordeon_contents = document.querySelectorAll('.accordeon-content');
    var accordeon_contents_child = document.querySelectorAll('.accordeon-content-child');
    var accordeon_items = document.querySelectorAll('.accordeon-item');

    for (var i = 0; i < acc.length; i++) {
        acc[i].onclick = function () {
            var setClasses = !this.classList.contains('active');
            setClass(acc, 'active', 'remove');
            setClass(panel, 'show', 'remove');

            if (setClasses) {
                this.classList.toggle("active");
                this.nextElementSibling.classList.toggle("show");
            }
        }
    }

    for (var i = 0; i < accordeon_titles.length; i++) {
        accordeon_titles[i].onclick = function () {
            var setClasses = !this.classList.contains('active');
            setClass(accordeon_titles, 'active', 'remove');
            setClass(accordeon_contents, 'show', 'remove');

            if (setClasses) {
                this.classList.toggle("active");
                this.nextElementSibling.classList.toggle("show");
            }
        }
    }

    for (var i = 0; i < accordeon_titles_child.length; i++) {
        accordeon_titles_child[i].onclick = function () {
            var setClasses = !this.classList.contains('active');
            setClass(accordeon_titles_child, 'active', 'remove');
            setClass(accordeon_contents_child, 'show', 'remove');

            if (setClasses) {
                this.classList.toggle("active");
                this.nextElementSibling.classList.toggle("show");
            }
        }
    }
}


let setClass = (els, className, fnName)  => {
    for (var i = 0; i < els.length; i++) {
        els[i].classList[fnName](className);
    }
}




function mobile_menu_toggle() {

    var toggle = document.getElementById('toggle-menu');
    var mobMenuWrap = document.getElementById("mobile-menu");
    var layerOff = document.getElementById("konn-off-layer");
    var close = document.getElementById('btn-close');

    toggle?.addEventListener('click', function () {
        mobMenuWrap?.classList?.toggle('open');
        layerOff?.classList?.toggle('visible');
    });

    close?.addEventListener('click', function () {
        mobMenuWrap?.classList?.toggle('open');
        layerOff?.classList?.toggle('visible');
    });

}


let intiSlid = () => {
    hideSlide();
    dotdisplay();
    shownSlide();  
    showAccordContent();
    mobile_menu_toggle();
}

var setActiveSlide = (lastIndex, currentIndex) => {
    var dots = document.querySelectorAll("#dots-item li.dot");
    dots[lastIndex].classList.remove("active");
    dots[currentIndex].classList.add("active");
}

window.addEventListener('load', intiSlid())