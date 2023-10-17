//function openDetails(cityName) {
//    var i;
//    var x = document.getElementsByClassName("product-item");
//    for (i = 0; i < x.length; i++) {
//      x[i].style.display = "none";  
//    }
//    document.getElementById(cityName).style.display = "block";  
//  }

var products_desc = document.getElementsByClassName("product-descr");
var next = document.getElementById("next");
var prev = document.getElementById("prev");

var functionListener = {

   showDescriProduct: (ev) => {
     let element = ev.target;
     if(element.className) {
     }
     else {
        element=element.parentNode
        if (element.classList.contains('active')){
            element.classList.remove('active')
        }

        else {
            element.classList.add('active');
        }
     }
     
    },

   shownSlide : (index) => {
    var sliders = document.getElementsByClassName("sec-desc-categ");  
    if (typeof(index) !== "number"){
        return
    }
    index = index%sliders.length;
    var lastIndex = Slideindex;
    Slideindex = index + 1;
    sliders[lastIndex]?.style.display = "none";
    sliders[Slideindex]?.style.display = "flex";
    setActiveSlide(lastIndex, Slideindex);
    },

    changeSlide : () => {
        var index = Slideindex
        shownSlide(index) 
    },
    
    nextSlide : () => {
        var index = Slideindex + 1 ;
        shownSlide(index);
    },
    prevSlide : () => {
        var index = Slideindex - 1 ;
       shownSlide(index)
    },

    setActiveSlide : (lastIndex, currentIndex) => {
        var dots = document.getElementsByClassName("dot");
        console.log(dots);
        dots[lastIndex].classList.remove("active");
        dots[currentIndex].classList.add("active");
    }
}

let setupListenerFunction = () => {
    // accordeon 

}

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

var MyBtn = document.getElementsByClassName("mybtn");
var index = 0 ;
function Button(n){
    CurrentShowButton(index = n);
}
function CurrentShowButton(n){
    for(var i = 0 ; i < MyBtn.length ; i++){
        MyBtn[i].className = MyBtn[i].className.replace(" Active","");
    }
    MyBtn[n].className +=" Active";
}

function List(){
    var Item = document.getElementById("item");
    var Item1 = document.getElementById("item1");
    var MyItem = document.getElementsByClassName("myItem");
    var Products = document.getElementsByClassName("product");

    for(var i = 0 ; i < MyItem.length ; i++){
        MyItem[i].style.margin = "10px 0";
    }
    for(var i = 0 ; i < Products.length ; i++){
        Products[i].style.flexDirection = "row";
    }
    Item.style.display = "block";
    Item1.style.display = "block";
    
}
function Grid(){
    var Item = document.getElementById("item");
    var Item1 = document.getElementById("item1");
    var MyItem = document.getElementsByClassName("myItem");
    var Products = document.getElementsByClassName("product");

    for(var i = 0 ; i < MyItem.length ; i++){
        MyItem[i].style.margin = "0px";
    }
    for(var i = 0 ; i < Products.length ; i++){
        Products[i].style.flexDirection = "column";
    }
    Item.style.display = "grid";
    Item1.style.display = "grid";
}

function filterToggle() {
    var closeFilter = document.getElementById("btn-close-filter")
    var contentFilter = document.getElementById('sidebar-main');
    contentFilter.classList.toggle('show');
    var layerOff = document.getElementById("konn-off-layer");
    layerOff.classList.toggle('visible');
    closeFilter.onclick = function (e) {
        contentFilter.classList.remove('show');
        layerOff.classList.remove('visible');
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
    sliders[lastIndex]?.style.display = "none";
    sliders[Slideindex]?.style.display = "flex";
    setActiveSlide(lastIndex, Slideindex);
}

function nextSlide() {
    var next = document.getElementById("next");
    var index = Slideindex + 1 ;
    shownSlide(index);
}

function prevSlide() {
    var prev = document.getElementById("prev");
    var index = Slideindex + 1 ;
    shownSlide(index);
}

var changeSlide = () => {
    var index = Slideindex 
    shownSlide(index)
}


let intiSlider = () => {
    hideSlide();
    shownSlide();
    dotdisplay();
    nextSlide();
    prevSlide();
}

var setActiveSlide = (lastIndex, currentIndex) => {
    var dots = document.querySelectorAll("#dots-item li.dot");
    dots[lastIndex].classList.remove("active");
    dots[currentIndex].classList.add("active");
}



//accordeon
function showAccordeonContent(ev) {
    var accordeon_titles = document.querySelectorAll('.accordeon-title');
    //console.log(accordeon_titles);
    var accordeon_contents = document.querySelectorAll('.accordeon-content');
    var accordeon_items = document.querySelectorAll('.accordeon-item');

    for (var i = 0; i < accordeon_titles.length; i++) {
        accordeon_titles[i].onclick = function(ev) {
            let element = ev.target;
            if(element.className == null) {
                element = (element.parentNode).parentNode
                //console.log(element);
            }else {
                //h2
                element = (element.parentNode).parentNode
            }
            var setClasses = !element.classList.contains('active');
            //console.log(setClasses);
            //var setClasses = !element.classList.contains('active');
            setClass(accordeon_contents, 'show', 'remove');
            
               if (setClasses) {
                this.classList.toggle("active");
                //this.classList.toggle("show");
                this.nextElementSibling.classList.toggle("show");
            }
        }
    }
   }

function setClass(els, className, fnName) {
    for (var i = 0; i < els.length; i++) {
        els[i].classList[fnName](className);
    }
}





window.onload = function () {
   // mobile_menu_toggle();
    showAccordeonContent();
    intiSlider();
    //setupListenerFunction();
    setClass();
    //openDetails();
    CurrentShowButton();
    List();
    Grid();
    filterToggle();
   //autoPlay();
};





