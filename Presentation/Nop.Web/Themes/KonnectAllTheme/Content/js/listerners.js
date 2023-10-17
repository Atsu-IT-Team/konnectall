//var Slideindex = 0;
var accordeon_items = document.querySelectorAll('.accordeon-item');
var sidebar_items = document.querySelectorAll('.sidebar-title');
var filter_toggle = document.getElementById("filter-toggle");

var functionListener = {
   
   showAccordeonContent : (ev) => {
    let element = ev.target;
    if(element.className) {
        // accordeon-title
        if(element.className === "accordeon-title") {
            element.element.parentNode
        }
        // accordeon-item
    }else {
        //h2
        element = (element.parentNode).parentNode
    }

    var accordeon_content = element.children[1]
    if (window.getComputedStyle(accordeon_content).display ){

        if(window.getComputedStyle(accordeon_content).display=="block"){
            accordeon_content.style.display = "none"
        }else {
            accordeon_content.style.display = "block"
        }
        
    }else{
        accordeon_content.style.display = "block"
    }

    element.classList.toggle('active');
   // console.log(element);
   },

   showsidebar : (ev) => {
    let elet = ev.target;
 
    if(elet.className) {
        if(elet.className !== "sidebar-title") {
            elet=(elet.parentNode).parentNode
            console.log(elet);
        }
    }
    elet.classList.toggle('inactive');
   },

   

   shownSlide : (index) => {
    var sliders = document.getElementsByClassName("sec-desc-categ");  
    if (typeof(index) !== "number"){
        return
    }
    index = index%sliders.length;
    var lastIndex = Slideindex;
    Slideindex = index + 1;
    console.log(Slideindex);
    sliders[lastIndex].style.display = "none";
    sliders[Slideindex].style.display = "flex";
    setActiveSlide(lastIndex, Slideindex);
    },

    

    changeSlide : () => {
        var index = Slideindex
        shownSlide(index) 
    },

    setActiveSlide : (lastIndex, currentIndex) => {
        var dots = document.getElementsByClassName("dot");
        console.log(dots);
        dots[lastIndex].classList.remove("active");
        dots[currentIndex].classList.add("active");
    }

}

var setupListenerFunction = () => {
    
    for (let index = 0; index < accordeon_items.length; index++) {
        const accordeon_item = accordeon_items[index];
        accordeon_item.onclick = functionListener.showAccordeonContent;
    }

    
    for (let index = 0; index < sidebar_items.length; index++) {
        const sidebar_item = sidebar_items[index];
        sidebar_item.onclick = functionListener.showsidebar;
    }

    
    if (filter_toggle === null) {
        console.log("null");

    }else {
        filter_toggle.onclick = function() {
            // var sidebar = document.getElementById("sidebar-content");
            filter_toggle.classList.toggle('active');
         }
    }
    

   /* var sliders = document.getElementsByClassName("sec-desc-categ");
    for (let index = 0; index < sliders.length; index++) {
       var slid = sliders[index];
        slid.style.display ="none";
    }

    var sliders = document.getElementsByClassName("sec-desc-categ");
    for (let index = 0; index < sliders.length; index++) {
        var slid = sliders[index];  
        var li = document.createElement('li');
        li.setAttribute('class', 'dot');
        document.getElementById('dots-item').appendChild(li);
        if(index === 0) {
            li.classList.add("active");
        }
        li.onclick = () => {
            functionListener.shownSlide(index);
        }
    }*/

    


}

