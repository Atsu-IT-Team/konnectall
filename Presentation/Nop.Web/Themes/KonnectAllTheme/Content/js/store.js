if(document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready )
} else {
    ready()
}

function ready() {
    var removeCartItelButtons = document.getElementsByClassName('shopping-cart-delete')
    console.log(removeCartItelButtons);
    for (var x=0; x<removeCartItelButtons.length; x++){
        console.log(removeCartItelButtons.length);
        var button = removeCartItelButtons[x];
        console.log(button)
        button.addEventListener('click', removeCartItem)
    };

    var quantityInputs = document.getElementsByClassName('cart-quantity-input')
    for (var i=0; i<quantityInputs.length; i++) {
        var input = quantityInputs[i]
        input.addEventListener('change', quantitychanged) 
    }

    var addToCartButtons = document.getElementsByClassName('shop-item-button')
    for (var i=0; i<addToCartButtons.length; i++) {
        var button = addToCartButtons[i]
        button.addEventListener('click', addToCartClicked)

    }
}

function removeCartItem(event) {
    var buttonclicked = event.target
    buttonclicked.parentNode.parentNode.remove()
    updateCartTotal()
}

function addToCartClicked(event) {
    var button = event.target
   
    var shopItem = button.parentNode.parentNode
    console.log('btn',shopItem)
    var title = shopItem.getElementsByClassName("shop-item-title")[0].innerText
    var price = shopItem.getElementsByClassName("shop-item-price")[0].innerText
    var imagesrc = shopItem.getElementsByClassName("shop-item-image")[0].src   
    addItemToCart(title, price, imagesrc)
    updateCartTotal()
}

function addItemToCart(title, price, imagesrc) {
    var cartRow = document.createElement('li')
    cartRow.classList.add('cart-row')
    var cartItems = document.getElementsByClassName("cart-items")[0]
    console.log(cartItems);
    var cartNameItems = cartItems.getElementsByClassName("cart-item-title")
    console.log(cartNameItems);
    for(var j = 0; j < cartNameItems.length; j++) {
        titleCart = cartNameItems[j].textContent.replace(/(\r\n|\n|\r)/gm,"")
        /*console.log(titleCart)
        console.log(title)
        console.log ("Tropicana" == titleCart.replaceAll(' ', ''))*/
        if(titleCart.replaceAll(' ', '') == title) {
           alert('this item is already added to the cart')
            return
        }
    }
    var cartRowContents = `
        <div class="shopping-cart-img">
                <img alt="" src="${imagesrc}">
        </div>

        <div class="shopping-cart-title">
            <div class="cart-item-title">
            ${title}
            </div>
            <h6>
            <input class="cart-quantity-input" type="number" value="2">
                <span class="cart-price">${price}</span>
            </h6>
        </div>
        <div class="shopping-cart-delete">
            <i class="fas fa-times"></i>
        </div>`
    cartRow.innerHTML = cartRowContents
    cartItems.append(cartRow)
    cartRow.getElementsByClassName('shopping-cart-delete')[0].addEventListener('click',removeCartItem)
    cartRow.getElementsByClassName('cart-quantity-input')[0].addEventListener('click',quantitychanged)

    document.getElementsByClassName('cart-counter')[0].textContent = cartNameItems.length
}

function quantitychanged(event) {
    var input = event.target
    if (isNaN(input.value) || input.value<=0) {
        input.value = 1
    }
    updateCartTotal()
}

function updateCartTotal() {
    var cartItemContainer = document.getElementsByClassName('cart-items')[0]
    var cartRows = document.getElementsByClassName('cart-row')
    var total = 0
    for(i=0; i<cartRows.length; i++) {
        var cartRow = cartRows[i]
        var priceElement = cartRow.getElementsByClassName('cart-price')[0]
        var quantityElement = cartRow.getElementsByClassName('cart-quantity-input')[0]
        var price = parseFloat(priceElement.textContent.replace('SAR',''))
        var quantity = quantityElement.value
        total = total + (price * quantity);
    }
    total = Math.round(total * 100) / 100
    document.getElementsByClassName('cart-total-price')[0].textContent = 'SAR' + total
}