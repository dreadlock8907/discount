using System;
using System.Collections.Generic;

namespace HomeTask
{
  interface IItem
  {
    void SetBasePrice(int price);
    double GetTotalPrice();
    bool HasDiscount();
  }

  class Item : IItem
  {
    private int _base_price;

    public Item(int price)
    {
      SetBasePrice(price);
    }

    public void SetBasePrice(int price)
    {
      _base_price = price;
    }

    public double GetTotalPrice()
    {
      return _base_price;
    }

    public bool HasDiscount()
    {
      return false;
    }
  }
  
  class DiscountedItem : IItem
  {
    private int _discount_percent;
    private IItem _item;

    public DiscountedItem(IItem item, int discount_percent)
    {
      _item = item;
      _discount_percent = discount_percent;
    }

    public void SetBasePrice(int price)
    {
      _item.SetBasePrice(price);
    }

    public double GetTotalPrice()
    {
      return _item.GetTotalPrice() - _item.GetTotalPrice() / 100 * _discount_percent;
    }

    public bool HasDiscount()
    {
      return true;
    }
  }
  
  static class DeliveryWayProvider
  {
    public static IEnumerable<DeliveryWay> GetPossibleWays(IItem item)
    {
      List<DeliveryWay> ways = new List<DeliveryWay>();
      ways.Add(new OnTheSpotDeliveryWay(item));
      if(!item.HasDiscount())
        ways.Add(new AddressedDeliveryWay(item));
      return ways;
    }
  }

  abstract class DeliveryWay
  {
    protected IItem _item;

    public DeliveryWay(IItem item)
    {
      _item = item;
    }

    public abstract void Deliver();
  }

  class OnTheSpotDeliveryWay : DeliveryWay
  {
    public OnTheSpotDeliveryWay(IItem item) : base(item)
    {
    }

    public override void Deliver()
    {
      Console.WriteLine($"Claimed by customer");
    }
  }
  
  class AddressedDeliveryWay : DeliveryWay
  {
    public AddressedDeliveryWay(IItem item) : base(item)
    {
    }

    public override void Deliver()
    {
      Console.WriteLine($"Delivered to address");
    }
  }
}