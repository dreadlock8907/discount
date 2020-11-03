using System;
using System.Collections.Generic;

namespace hometask
{
  public interface IItem
  {
    void SetBasePrice(int price);
    double GetPrice();
    bool HasDiscount();
  }

  public class Item : IItem
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

    public double GetPrice()
    {
      return _base_price;
    }

    public bool HasDiscount()
    {
      return false;
    }
  }
  
  public class DiscountedItem : IItem
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

    public double GetPrice()
    {
      return _item.GetPrice() - _item.GetPrice() / 100 * _discount_percent;
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
      ways.Add(new PickUpPointDeliveryWay(item));
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

  class PickUpPointDeliveryWay : DeliveryWay
  {
    public PickUpPointDeliveryWay(IItem item) : base(item)
    {
    }

    public override void Deliver()
    {
      Console.WriteLine("Claimed by customer");
    }
  }
  
  class AddressedDeliveryWay : DeliveryWay
  {
    public AddressedDeliveryWay(IItem item) : base(item)
    {
    }

    public override void Deliver()
    {
      Console.WriteLine("Delivered to address");
    }
  }

  public class ItemShowcase
  {
    Item _item;
    readonly List<IItem> _discounted_items;
    
    public ItemShowcase(Item item)
    {
      _item = item;
      _discounted_items = new List<IItem>()
      {
        new DiscountedItem(_item, discount_percent: 10),
        new DiscountedItem(_item, discount_percent: 20),
        new DiscountedItem(_item, discount_percent: 30)
      };
    }

    public void UpdateBasePrice(int new_price)
    {
      _item.SetBasePrice(new_price);
    }

    public void ShowAllPrices()
    {
      for (int i = 0; i < _discounted_items.Count; i++)
        ShowPriceTag(_discounted_items[i]);
    }
    
    void ShowPriceTag(IItem item)
    {
      Console.WriteLine($"Item price: {item.GetPrice()}");
    }
  }

  public static class Test
  {
    public static void Run()
    {
      ItemShowcase showcase = new ItemShowcase(new Item(100));
      showcase.ShowAllPrices();
      showcase.UpdateBasePrice(200);
      showcase.ShowAllPrices();
    }
  }
  
  
}