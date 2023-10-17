
class MyNewsletter extends HTMLElement {
    connectedCallback() {
        this.innerHTML = `
        <div class="newsletter">
            <div class="container">
                <div class="content-newsletter row">
                    <div class="col-md-6">
                        <h3 class="text-left">
                            Join our Newsletter!
                        </h3>
                        <p data-qe-id="form-description" class="">
                            Subscribe to the newsletter to be the first to know what's new,
                            specials and exclusive promotions! 
                        </p>
                    </div>

                    <div class="col-md-6">
                        <form>
                            <input required="" type="email" name="email" placeholder="Enter your Email">
                            <a href="#" class="btn btn-primary text-black newsletter-btn">SUBSCRIBE</a>
                        </form>
                        <p>We care about the protection of your data</p>
                    </div>
                </div>
                <span class="arrow-bottom"></span>
            </div>
        </div>
        `
    }
}
customElements.define('my-newsletter', MyNewsletter)

class MiddleBanner extends HTMLElement {
    connectedCallback() {
        this.innerHTML = `
        <section class="container middle-banner">
            <img alt="" src="./assets/img/b2b-category/banner-middle.png" class="banner-middle">

            <div class="content-banner">
                <div class="number text-yellow-light font-bold">25</div>
                <h3 class="percent text-uppercase text-yellow-light">% off</h3>
                <div class="line-separate"></div>
                <div class="desc-content">
                    <h3 class="text-capitalize text-white">fresh green vegetables</h3>
                    <a href="#" class="btn-sm-white">
                        <span class="text-white font-normal">shop now</span>
                    </a>
                </div>
            </div>
        </section>
        `
    }
}
customElements.define('middle-banner', MiddleBanner)
